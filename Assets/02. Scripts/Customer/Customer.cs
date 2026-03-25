using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CustomerState
{
    MovingToQueue, WaitingForStock, Buying, Leave
}

public class Customer : PoolObject
{
    CustomerMover mover;
    QueueManager queueMgr;
    SellZone sellZone;

    public CustomerState State {  get; private set; }

    Transform leavePoint;

    bool isArrived;
    int targetBuyAmount;
    int curBoughtAmount;

    public bool IsArrived => isArrived;
    public Action<Customer> OnArrived;

    public float buyDelay = .2f;

    private void Awake()
    {
        mover = GetComponent<CustomerMover>();
    }

    private void Update()
    {
        if (!isArrived && mover.IsArrived())
        {
            isArrived = true;
            OnArrived?.Invoke(this);
        }
    }

    public void Init(QueueManager mgr, SellZone sellZone, Transform leavePoint)
    {
        queueMgr = mgr;
        this.leavePoint = leavePoint;
        this.sellZone = sellZone;
        targetBuyAmount = 4;
        curBoughtAmount = 0;

        // 초기 상태 지정
        State = CustomerState.MovingToQueue;

        queueMgr.AddCustomer(this);
    }

    public void TryStartBuying()
    {
        if (sellZone.curStock > 0)
        {
            StartBuying();
        }
        else
        {
            WaitForStock();
        }
    }

    public void StartBuying()
    {
        // 반복 호출되므로 조건 필요
        if (State == CustomerState.Buying) return;

        State = CustomerState.Buying;

        StartCoroutine(BuyRoutine());
    }

    void WaitForStock()
    {
        State = CustomerState.WaitingForStock;

        sellZone.OnStockAvailable += HandleStockAvailable;
    }

    void HandleStockAvailable()
    {
        sellZone.OnStockAvailable -= HandleStockAvailable;

        StartBuying();
    }

    IEnumerator BuyRoutine()
    {
        while (curBoughtAmount < targetBuyAmount)
        {
            int remain = targetBuyAmount - curBoughtAmount;

            // 일부 구매 기능
            int sold = sellZone.TrySell(remain);

            if (sold > 0)
            {
                curBoughtAmount += sold;


                yield return new WaitForSeconds(.2f);
            }
            else
            {
                // 재고 없음 -> 기다림
                yield return new WaitForSeconds(.1f);
            }
        }

        Leave();
    }

    public void SetTarget(Vector3 pos)
    {
        isArrived = false;

        mover.SetTarget(pos);
    }

    public void Leave()
    {
        State = CustomerState.Leave;

        mover.SetTarget(leavePoint.position);

        queueMgr.RemoveCustomer(this);

        StartCoroutine(LeaveRoutine());
    }

    IEnumerator LeaveRoutine()
    {
        // 도착 안 했으면 기다리기
        while (!mover.IsArrived())
            yield return null;

        // 도착하면 풀로 반환
        ReturnPool();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CustomerState
{
    MovingToQueue,
    WaitingForStock,
    Buying,
    Leave
}

public class Customer : PoolObject
{
    CustomerMover mover;

    QueueManager queueMgr;
    SellZone sellZone;
    Transform leavePoint;

    public CustomerState State { get; private set; }

    int targetBuyAmount;
    int curBoughtAmount;

    bool isArrived;
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

    #region === Init ===

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

    #endregion
    #region === Buying ===

    public void TryStartBuying()
    {
        if (sellZone.curStock > 0)
            StartBuying();
        else
            WaitForStock();
    }

    public void StartBuying()
    {
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
            int sold = sellZone.TrySell(remain);

            if (sold > 0)
            {
                curBoughtAmount += sold;
                yield return new WaitForSeconds(buyDelay);
            }
            else
            {
                yield return new WaitForSeconds(.1f);
            }
        }

        Leave();
    }

    #endregion

    #region === Movement ===

    public void SetTarget(Vector3 pos)
    {
        isArrived = false;
        mover.SetTarget(pos);
    }

    #endregion

    #region === Leave ===

    public void Leave()
    {
        State = CustomerState.Leave;

        mover.SetTarget(leavePoint.position);
        queueMgr.RemoveCustomer(this);

        StartCoroutine(LeaveRoutine());
    }

    IEnumerator LeaveRoutine()
    {
        while (!mover.IsArrived())
            yield return null;

        ReturnPool();
    }

    #endregion
}

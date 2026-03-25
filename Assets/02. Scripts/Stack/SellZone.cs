using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellZone : MonoBehaviour
{
    public MoneyZone moneyZone;
    DropZone drop;

    public int curStock;
    public event Action OnStockAvailable;

    private void Awake()
    {
        drop = GetComponent<DropZone>();
    }

    public void AddStock(int amount)
    {
        bool wasEmpty = curStock == 0;

        curStock += amount;

        if (wasEmpty && curStock > 0)
        {
            OnStockAvailable?.Invoke();
        }
    }

    // Customer 구매 시 호출
    public int TrySell(int RequestAmount)
    {
        if (curStock <= 0)
            return 0;

        int soldAmount = Mathf.Min(RequestAmount, curStock);

        drop.SetDropZoneCount(-soldAmount);

        StartCoroutine(SellRoutine(soldAmount));

        return soldAmount;
    }

    IEnumerator SellRoutine(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject go = drop.PopItem();

            if (go == null) yield break;

            curStock--;

            // 돈 생성
            moneyZone.AddMoney();

            go.SetActive(false);

            yield return new WaitForSeconds(.3f);
        }

    }
}

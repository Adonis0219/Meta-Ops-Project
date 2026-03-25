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

    public int TrySell(int RequestAmount)
    {
        int dropCount = drop.DropZoneCount;

        if (dropCount <= 0)
            return 0;

        int soldAmount = Mathf.Min(RequestAmount, dropCount);

        drop.SetDropZoneCount(-soldAmount);

        StartCoroutine(RemoveRoutine(soldAmount));

        return soldAmount;
    }

    IEnumerator RemoveRoutine(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // 아이템 제거
            drop.RemoveItem();

            curStock--;

            moneyZone.AddMoney();

            yield return new WaitForSeconds(.3f);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    #region === Inspector ===

    public QueueManager queueManager;
    public SellZone sellZone;

    public Transform spawnPoint;
    public Transform leavePoint;

    public int maxCount = 4;

    #endregion

    Customer lastCustomer;

    private void Start()
    {
        // Ã¹ ½ºÆù
        Spawn();
    }

    #region === Spawn ===

    void Spawn()
    {
        if (queueManager.Count >= maxCount) return;

        GameObject obj = PoolManager.instance.GetPool(PoolObejectType.Customer);
        obj.transform.position = spawnPoint.position;

        Customer customer = obj.GetComponent<Customer>();
        customer.Init(queueManager, sellZone, leavePoint);

        RegisterLastCustomer(customer);
    }

    void RegisterLastCustomer(Customer customer)
    {
        if (lastCustomer != null)
            lastCustomer.OnArrived -= HandleArrived;

        lastCustomer = customer;
        customer.OnArrived += HandleArrived;
    }

    void HandleArrived(Customer customer)
    {
        Spawn();
    }

    #endregion
}

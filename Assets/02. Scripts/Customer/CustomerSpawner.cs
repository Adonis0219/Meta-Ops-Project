using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject prefab;
    public QueueManager queueManager;
    public SellZone sellZone;

    public Transform spawnPoint;
    public Transform leavePoint;

    public int maxCount = 4;

    Customer lastCustomer;

    private void Start()
    {
        // 첫 스폰
        Spawn();
    }

    void Spawn()
    {
        if (queueManager.Count >= maxCount) return;

        GameObject obj = PoolManager.instance.GetPool(PoolObejectType.Customer);
        obj.transform.position = spawnPoint.position;

        Customer customer = obj.GetComponent<Customer>();
        customer.Init(queueManager, sellZone, leavePoint);

        RegistLastCustomer(customer);
    }

    void RegistLastCustomer(Customer customer)
    {
        // 이전 구독 해제
        if (lastCustomer != null)
            lastCustomer.OnArrived -= HandleArrived;

        // 마지막 손님 등록
        lastCustomer = customer;

        // 구독
        customer.OnArrived += HandleArrived;
    }

    void HandleArrived(Customer customer) => Spawn();
}

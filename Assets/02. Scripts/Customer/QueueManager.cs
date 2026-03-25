using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public Transform counterPoint;
    public float spacing = 1.5f;

    List<Customer> customers = new List<Customer>();

    public int Count => customers.Count;

    public void AddCustomer(Customer customer)
    {
        customers.Add(customer);

        customer.OnArrived += HandleCustomerArrived;

        UpdateQueue();
    }

    public void RemoveCustomer(Customer customer)
    {
        customer.OnArrived -= HandleCustomerArrived;

        UpdateQueue();

        customers.Remove(customer);
    }

    public Customer GetLastCusomer()
    {
        if (customers.Count == 0) return null;
        // ^1 = 마지막 원소 ^2 = 뒤에서 두번째
        // 리스트가 비어있으면 error가 나므로 리스트 검사 필수
        return customers[^1];
    }

    void UpdateQueue()
    {
        for (int i = 0; i < customers.Count; i++)
        {
            Vector3 targetPos;

            if (i == 0)
            {
                targetPos = counterPoint.position;
            }
            else
            {
                Transform front = customers[i - 1].transform;

                targetPos = counterPoint.position - counterPoint.forward * spacing * i;

                //Debug.Log($"Customer {i} target: {front.gameObject.name}");
            }


            customers[i].SetTarget(targetPos);
        }
    }

    void HandleCustomerArrived(Customer customer)
    {
        // 맨 앞 손님인지 확인
        if (customers.Count == 0) return;

        if (customers[0] == customer && customer.State != CustomerState.Buying) customer.TryStartBuying();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public Transform counterPoint;
    public float spacing = 1.5f;

    List<Customer> customers = new List<Customer>();

    public int Count => customers.Count;

    #region === Customer ===

    public void AddCustomer(Customer customer)
    {
        customers.Add(customer);

        customer.OnArrived += HandleCustomerArrived;

        UpdateQueue();
    }

    public void RemoveCustomer(Customer customer)
    {
        customer.OnArrived -= HandleCustomerArrived;

        customers.Remove(customer);

        UpdateQueue();
    }

    public Customer GetLastCustomer()
    {
        if (customers.Count == 0) return null;
        return customers[^1];
    }

    #endregion

    #region === Queue ===

    void UpdateQueue()
    {
        for (int i = 0; i < customers.Count; i++)
        {
            Vector3 targetPos =
                counterPoint.position - counterPoint.forward * spacing * i;

            customers[i].SetTarget(targetPos);
        }
    }

    void HandleCustomerArrived(Customer customer)
    {
        if (customers.Count == 0) return;

        if (customers[0] == customer &&
            customer.State != CustomerState.Buying)
        {
            customer.TryStartBuying();
        }
    }

    #endregion
}
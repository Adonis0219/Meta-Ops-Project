using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Customer : MonoBehaviour
{
    CustomerMover mover;
    QueueManager queueMgr;

    bool isArrived;

    public bool IsArrived => isArrived;

    private void Awake()
    {
        mover = GetComponent<CustomerMover>();
    }

    private void Update()
    {
        if (!isArrived && mover.IsArrived())
        {
            isArrived = true;
        }
    }

    public void Init(QueueManager mgr)
    {
        queueMgr = mgr;
        queueMgr.AddCustomer(this);
    }

    public void SetTarget(Vector3 pos)
    {
        mover.SetTarget(pos);
    }

    public void Leave()
    {
        queueMgr.RemoveCustomer(this);
        Destroy(gameObject);
    }
}

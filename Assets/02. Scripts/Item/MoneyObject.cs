using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyObject : Item
{
    Vector3 target;
    float speed = 10f;

    public event Action OnArrived;

    public void InitBehaviour(Vector3 start, Vector3 target)
    {
        transform.position = start;
        this.target = target;

        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        while (Vector3.Distance(transform.position, target) > .05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, target, speed * Time.deltaTime);

            yield return null;
        }

        OnArrived?.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMover : MonoBehaviour
{
    public float moveSpd = 3f;

    Vector3 target;

    private void Update()
    {
        Move();
    }

    #region === Move ===

    public void SetTarget(Vector3 targetPos) => target = targetPos;

    void Move()
    {
        float dist = Vector3.Distance(transform.position, target);

        if (dist > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, target, moveSpd * Time.deltaTime);

            transform.forward = (target - transform.position).normalized;
        }
    }

    public bool IsArrived()
    {
        return Vector3.Distance(transform.position, target) < 0.05f;
    }

    #endregion
}


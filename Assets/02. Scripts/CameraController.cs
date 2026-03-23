using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // 카메라가 따라갈 대상

    [Header("# Offset")]
    public Vector3 offset = new Vector3(0, 5, -10); // 대상과 카메라 사이의 오프셋

    // LateUpdate는 Update보다 나중에 호출되므로, 대상이 이동한 후에 카메라가 따라가도록 보장
    private void LateUpdate()
    {
        if (target == null) return;

        // 목표 위치
        transform.position = target.position + offset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class BaseZone : MonoBehaviour
{
    protected Color oriColor;
    protected Color activeColor = Color.green;

    protected Material mat;

    protected virtual void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
        oriColor = mat.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other)) return;

        mat.color = activeColor;

        OnEnterZone(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other)) return;

        mat.color = oriColor;

        OnExitZone(other);
    }

    // 공통 필터
    protected bool IsPlayer(Collider other)
    {
        return other.CompareTag("Player");
    }

    // 자식이 구현
    // 선택적 override(Hook) 패턴 -> 자식 클래스에서 필수 구현 X
    protected virtual void OnEnterZone(Collider other) { }
    protected virtual void OnExitZone(Collider other) { }
}

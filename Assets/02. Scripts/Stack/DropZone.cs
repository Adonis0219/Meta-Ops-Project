using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    public List<Transform> dropPoints = new List<Transform>(); // 드롭 지점 리스트
    public float dropZoneSpacing = 0.3f; // 드롭 지점 간격
    public float firstSpacing = .1f; // 드롭존과 첫 번째 아이템 간격

    Coroutine deliveryCoru; // 아이템 제거 코루틴 참조

    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Player")) return;

        var playerStack = other.GetComponent<StackSystem>();
        var playerInven = other.GetComponent<Inventory>();
        var delivery = GetComponent<DeliveryHandler>();

        if (playerStack == null || playerInven == null || delivery == null) return;

        deliveryCoru = StartCoroutine(delivery.Deliver(playerStack, this, playerInven));
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (deliveryCoru != null)
        {
            StopCoroutine(deliveryCoru);
             deliveryCoru = null; // 참조 초기화
        }
    }

    public void StackInDropZone(Transform _item)
    {
        for (int i = 0; i < dropPoints.Count; i++)
        {
            if (dropPoints[i].childCount < 10)
            {
                _item.SetParent(dropPoints[i], false);
                _item.localPosition = Vector3.zero;
                _item.localRotation = Quaternion.identity;
                _item.localScale = new Vector3(1, .5f, 1);

                break; // 아이템이 드롭존에 쌓였으므로 루프 종료
            }
        }

        UpdateStackPositions();
    }

    void UpdateStackPositions()
    {
        for (int i = 0; i < dropPoints.Count; i++)
        {
            for (int j = 0; j < dropPoints[i].childCount; j++)
            {
                Transform item = dropPoints[i].GetChild(j);
                Vector3 targetPosition = new Vector3(0, j * dropZoneSpacing + firstSpacing, 0);
                item.localPosition = targetPosition;
            }
        }
    }

    public Transform SelectDropPoint()
    {
        for (int i = 0; i < dropPoints.Count; i++)
        {
            if (dropPoints[i].childCount < 10)
                return dropPoints[i]; // 드롭 지점이 비어있으면 해당 인덱스 반환
        }

        return null; // 모든 드롭 지점이 가득 찼을 때 기본값으로 null 반환
    }
}

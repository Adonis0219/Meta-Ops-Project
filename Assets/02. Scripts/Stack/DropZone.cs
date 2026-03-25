using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class DropZone : BaseZone
{
    #region === Inspector ===

    public List<Transform> dropPoints = new List<Transform>(); // 드롭 지점 리스트
    public float dropZoneSpacing = 0.3f; // 드롭 지점 간격
    public float firstSpacing = .1f; // 드롭존과 첫 번째 아이템 간격

    public ProductZone productZone; // 가공 제품이 쌓이는 드롭존
    public SellZone sellZone;

    public PoolObejectType dropZoneType;

    #endregion

    Coroutine deliveryCoru; // 아이템 제거 코루틴 참조
    [SerializeField]
    int dropZoneCount = 0; // 드롭존에 쌓인 아이템 수

    public int DropZoneCount
    {
        get => dropZoneCount;
        private set => dropZoneCount = value;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        var playerStack = other.GetComponent<StackSystem>();
        var playerInven = other.GetComponent<Inventory>();
        var delivery = GetComponent<DeliveryHandler>();

        if (playerStack == null || playerInven == null || delivery == null) return;

        deliveryCoru = StartCoroutine(delivery.Deliver(playerStack, this, playerInven));       
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (deliveryCoru != null)
        {
            StopCoroutine(deliveryCoru);
             deliveryCoru = null; // 참조 초기화
        }
    }

    public void SetDropZoneCount(int amount)
    {
        dropZoneCount += amount;
    }

    public void StackInDropZone(Transform _item)
    {
        for (int i = 0; i < dropPoints.Count; i++)
        {
            if (dropPoints[i].childCount < 10)
            {
                Item itemComp = _item.GetComponent<Item>(); 

                _item.SetParent(dropPoints[i], false);
                _item.localPosition = Vector3.zero;
                _item.localRotation = Quaternion
                    .Euler(itemComp.obtainAngle.x, itemComp.obtainAngle.y, itemComp.obtainAngle.z);
                _item.localScale = itemComp.obtainScale;

                break; // 아이템이 드롭존에 쌓였으므로 루프 종료
            }
        }

        UpdateStackPositions();

        if (gameObject.name == "SellZone Obj")
        {
            sellZone.AddStock(1);
        }
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

    public GameObject PopItem()
    {
        for (int i = 0; i < dropPoints.Count; i++)
        {
            if (dropPoints[i].childCount > 0)
            {
                // 맨 위 아이템 가져오기
                Transform item = dropPoints[i].GetChild(dropPoints[i].childCount - 1);

                item.SetParent(item.GetComponent<Item>().parent); // 아이템의 부모를 원부모로 설정하여 드롭존에서 제거

                dropZoneCount--; // 드롭존에 아이템 개수 감소

                return item.gameObject; // 아이템 반환
            }
        }

        return null; // 모든 드롭 지점이 비어있을 때 기본값으로 null 반환
    }

    public void RemoveItem()
    {
        PopItem().SetActive(false);
    }
}

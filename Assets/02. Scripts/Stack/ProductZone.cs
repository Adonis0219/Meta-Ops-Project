using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProductZone : MonoBehaviour
{
    #region === Animation ===

    public Animator conv_anim;

    private const string RUN = "isRun";

    #endregion

    #region === Inspector ===

    public float productDelay = 1.5f; // 제품이 생산되는 시간 간격
    public DropZone dropZone; // 원재료(광석)가 쌓이는 드롭존
    public float prodZoneSpacing = 0.3f; // 제품 간 간격
    public float firstSpacing = .1f; // 바닥과 첫 번째 제품 사이 간격

    #endregion

    List<GameObject> products = new List<GameObject>();
    Coroutine produceCoru = null; // 생산 코루틴 참조

    public int ProdCount => products.Count;
    public IReadOnlyList<GameObject> Products => products;

    private void Update()
    {
        // 원재료가 존재하면 → 생산 시작
        if (dropZone.DropZoneCount != 0)
        {
            if (produceCoru != null) return; // 이미 생산 중이면 중복 실행 방지

            produceCoru = StartCoroutine(ProduceLoop());
            conv_anim.SetBool(RUN, true); // 생산 애니메이션 실행    
        }
        // 원재료가 없으면 → 생산 중단
        else
        {
            if (produceCoru == null) return;

            StopCoroutine(produceCoru);
            produceCoru = null; // 코루틴 참조 초기화
            conv_anim.SetBool(RUN, false); // 애니메이션 정지
        }
    }

    IEnumerator ProduceLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(productDelay); // 생산 간격 대기

            // 원재료가 없으면 생산 종료
            if (dropZone.DropZoneCount <= 0) yield break;

            // 원재료 소비
            dropZone.RemoveOre();

            // 제품 생성
            ProduceProduct();
        }
    }

    void ProduceProduct()
    {
        GameObject product = PoolManager.instance.GetPool(PoolObejectType.Product);

        // 제품 리스트에 추가
        products.Add(product);

        // ProductZone의 자식으로 설정 (스택 구조)
        product.transform.SetParent(transform);

        // 스택 위치 재정렬
        UpdateStackPositions();
    }

    void UpdateStackPositions()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform item = transform.GetChild(i);

            // 위로 쌓이도록 위치 설정
            Vector3 targetPosition = new Vector3(0, i * prodZoneSpacing + firstSpacing, 0);
            item.localPosition = targetPosition;
        }
    }

    public void Remove(GameObject item)
    {
        products.Remove(item);
    }
}
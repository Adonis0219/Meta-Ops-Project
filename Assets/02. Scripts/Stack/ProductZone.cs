using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductZone : MonoBehaviour
{
    #region === Animation ===

    public Animator conv_anim;

    private const string RUN = "isRun";

    #endregion

    #region === Inspector ===

    public float productDelay = 1.5f; // 제품이 생산되는 시간 간격
    public DropZone dropZone; // 제품이 쌓이는 드롭존

    #endregion

    Coroutine produceCoru = null; // 제품 생산 코루틴 참조

    private void Update()
    {
        // 광석 있음 -> 생산 시작
        if (dropZone.dropZoneCount != 0)
        {
            if (produceCoru != null) return; // 이미 생산 중이면 중복 실행 방지

            produceCoru = StartCoroutine(ProduceLoop());
            conv_anim.SetBool(RUN, true); // 애니메이션 재생    
        }
        else
        {
            if (produceCoru == null) return;

            StopCoroutine(produceCoru);
            produceCoru = null; // 참조 초기화
            conv_anim.SetBool(RUN, false); // 애니메이션 정지
        }
    }

    IEnumerator ProduceLoop()
    {
        while (true)
        {           
            yield return new WaitForSeconds(productDelay); // 제품이 생산되는 시간 간격 대기

            if (dropZone.dropZoneCount <= 0) yield break; // 드롭존에 아이템이 없으면 생산 종료

            // 실제 생산 처리
            dropZone.RemoveOre(); // 드롭존에서 광석 제거
            // 제품 생산
            ProduceProduct();
        }
    }

    void ProduceProduct()
    {

    }
}

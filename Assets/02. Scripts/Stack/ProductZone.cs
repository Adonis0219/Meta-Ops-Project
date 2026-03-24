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

    public void ProduceProduct()
    {
        if (conv_anim.GetBool(RUN)) return; // 이미 생산 중이면 중복 실행 방지

        // 제품 생산 애니메이션 재생
        conv_anim.SetBool(RUN, true);
        // 제품 생산 로직 실행

        StartCoroutine(ProduceProductCoru());
    }

    IEnumerator ProduceProductCoru()
    {
        yield return new WaitForSeconds(productDelay); // 제품이 생산되는 시간 간격 대기
        // 제품이 생산되면 드롭존에 추가
        
        // 애니메이션 종료
        conv_anim.SetBool(RUN, false);
    }
}

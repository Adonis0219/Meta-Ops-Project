using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    public IEnumerator PlayCollectEffect(Transform _target)
    {
        float duration = 0.2f;
        float time = 0;

        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = new Vector3(1, .5f, 1);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // 부드러운 가속
            t *= t;
            // 부드러운 이동
            transform.position = Vector3.Lerp(startPos, _target.position, t);
            // 부드러운 축소
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            yield return null;
        }
    }

    #region 필요 시 사용
    public IEnumerator StackMoveEffect(Transform _item, Vector3 _targetLocPos)
    {
        float duration = 0.15f;
        float time = 0;

        Vector3 startPos = _item.localScale + Vector3.up;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // 부드러운 감속
            t = 1 - Mathf.Pow(1 - t, 2);

            _item.localPosition = Vector3.Lerp(startPos, _targetLocPos, t);

            yield return null;
        }

        _item.localPosition = _targetLocPos;
    }
    #endregion
}

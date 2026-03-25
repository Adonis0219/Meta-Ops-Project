using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    public IEnumerator PlayCollectEffect(Transform _target)
    {
        float duration = .2f;
        float time = 0;

        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // 부드러운 가속
            t *= t;
            // 부드러운 이동
            transform.position = Vector3.Lerp(startPos, _target.position, t);

            yield return null;
        }
    }
}

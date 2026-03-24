using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class DeliveryHandler : MonoBehaviour
{
    public float deliveryTime = .1f; // 납품 시간

    public IEnumerator Deliver(StackSystem _stack, DropZone _drop, Inventory _inven)
    {
        while (true)
        {
            Transform item = _stack.PopItem();

            if (item == null)
                yield break; // 스택이 비어있으면 납품 종료

            var dropPoint = _drop.SelectDropPoint();

            if (dropPoint == null) yield break; // 납품 구역이 가득 찼으면 납품 종료

            yield return DeliveryEffect(item, _drop, dropPoint);

            Item itemCop = item.GetComponent<Item>();
            _inven.RemoveItem(itemCop.poolType, itemCop.scoreValue); // 인벤토리에서 아이템 제거
            _drop.dropZoneCount++; // 납품 구역에 아이템 개수 추가

            yield return new WaitForSeconds(deliveryTime); // 납품 시간 대기
        }
    }

    public IEnumerator DeliveryEffect(Transform _item, DropZone _drop, Transform _point)
    {
        float duration = 0.2f;
        float time = 0;

        Vector3 startPos = _item.position;
        Vector3 startScale = _item.localScale;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // 부드러운 가속
            t *= t;

            // 이동
            _item.position = Vector3.Lerp(startPos, _point.position, t);

            // 작아지기
            //_item.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            yield return null;
        }

        _drop.StackInDropZone(_item); // 아이템을 납품 구역에 놓음
    }
}

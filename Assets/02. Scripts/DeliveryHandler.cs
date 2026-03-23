using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            _drop.StackInDropZone(item); // 아이템을 납품 구역에 놓음
            _inven.Remove(1); // 인벤토리에서 아이템 제거

            yield return new WaitForSeconds(deliveryTime); // 납품 시간 대기
        }
    }
}

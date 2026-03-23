using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int count = 0; // 수집한 아이템 수
    public int maxCount = 10; // 최대 수집 가능한 아이템 수
    public bool IsFull => Count >= maxCount; // 인벤토리가 가득 찼는지 여부

    // 추후 확장성 고려(UI 업데이트 등)하여 프로퍼티로 구현
    public int Count
    {
        get => count;
        private set
        {
            count = Mathf.Clamp(value, 0, maxCount); // Count가 0과 maxCount 사이에 있도록 제한
        }
    }

    public void Add(int _amount)
    {
        Count += _amount;

        Debug.Log($"현재 수집한 아이템 수: {Count}");
    }
}

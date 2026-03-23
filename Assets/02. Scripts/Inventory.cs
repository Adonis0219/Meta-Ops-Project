using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int count = 0; // 수집한 아이템 수
    public int maxCount = 10; // 최대 수집 가능한 아이템 수

    // 추후 확장성 고려(UI 업데이트 등)하여 프로퍼티로 구현
    public int Count
    {
        get => count;
        private set
        {
            count = Mathf.Clamp(value, 0, maxCount); // Count가 0과 maxCount 사이에 있도록 제한
            Check_Max();
        }
    }

    public void Add(int _amount)
    {
        Count += _amount;

        Debug.Log($"현재 수집한 아이템 수: {Count}");
    }

    void Check_Max()
    {
        if (Count >= maxCount)
        {
            Debug.Log("최대 수집 아이템 수에 도달했습니다!");
            // 추가 기능 구현부(예: UI 업데이트, 게임 승리 조건 체크 등)
        }
    }
}

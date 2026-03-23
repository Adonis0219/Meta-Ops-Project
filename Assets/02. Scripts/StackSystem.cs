using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackSystem : MonoBehaviour
{
    public Transform stackRoot;
    public float stackSpacing = 1.0f;

    private List<Transform> stackItems = new List<Transform>();

    /// <summary>
    /// 스택에 받아온 아이템을 추가하고, 쌓은 모든 항목의 위치를 업데이트 합니다.
    /// </summary>
    /// <param name="item">스택에 추가할 아이템, null이 될 수 없음</param>
    public void AddToStack(Transform item)
    {
        item.SetParent(stackRoot, false);

        item.localPosition = Vector3.zero; // 초기 위치는 루트에 맞춤
        item.localRotation = Quaternion.identity; // 회전 초기화
        item.localScale = new Vector3(1, .5f, 1); // 아이템 크기 조정

        stackItems.Add(item);

        UpdateStackPositions();
    }

    /// <summary>
    /// 현재 순서와 간격을 토대로 스택에 있는 모든 오브젝트의 위치를 수정합니다
    /// </summary>
    void UpdateStackPositions()
    {
        for (int i = 0; i < stackItems.Count; i++)
        {
            Vector3 targetPosition = new Vector3(0, i * stackSpacing, 0);
            Debug.Log(i + "번 아이템 위치: " + targetPosition);
            stackItems[i].localPosition = targetPosition;
        }
    }
}

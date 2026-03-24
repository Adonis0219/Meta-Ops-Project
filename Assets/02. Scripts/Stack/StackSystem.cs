using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackSystem : MonoBehaviour
{
    public Transform stackRoot;
    public float stackSpacing = 1.0f;

    private List<Transform> stackItems = new List<Transform>();

    public void AddToStack(Transform item)
    {
        item.SetParent(stackRoot, false);

        item.localPosition = Vector3.zero; // 부모의 위치에 맞춰 초기 위치 설정
        item.localRotation = Quaternion.identity; // 부모의 회전에 맞춰 초기 회전 설정
        item.localScale = new Vector3(1, .5f, 1); // 아이템의 크기를 조정 (예시로 높이를 절반으로 줄임)

        stackItems.Add(item);

        UpdateStackPositions();
    }
    
    /// <summary>
    /// Removes and returns the top item from the stack.
    /// </summary>
    /// <returns>가장 상단 오브젝트의 <see cref="Transform"/>, 또는 스택이 비어있다면 <see langword="null"/> 반환.</returns>
    public Transform PopItem()
    {
        if (stackItems.Count == 0) return null;
        
        Transform topItem = stackItems[stackItems.Count - 1];
        stackItems.RemoveAt(stackItems.Count - 1);

        UpdateStackPositions();

        return topItem;
    }

    void UpdateStackPositions()
    {
        for (int i = 0; i < stackItems.Count; i++)
        {
            Vector3 targetPosition = new Vector3(0, i * stackSpacing, 0);
            stackItems[i].localPosition = targetPosition;
        }
    }
}

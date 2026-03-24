using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackSystem : MonoBehaviour
{
    public Transform stackRoot;
    public float stackSpacing = 1.0f;

    private List<Transform> stackItems = new List<Transform>();

    /// <summary>
    /// ���ÿ� �޾ƿ� �������� �߰��ϰ�, ���� ��� �׸��� ��ġ�� ������Ʈ �մϴ�.
    /// </summary>
    /// <param name="item">���ÿ� �߰��� ������, null�� �� �� ����</param>
    public void AddToStack(Transform item)
    {
        item.SetParent(stackRoot, false);

        item.localPosition = Vector3.zero; // �ʱ� ��ġ�� ��Ʈ�� ����
        item.localRotation = Quaternion.identity; // ȸ�� �ʱ�ȭ
        item.localScale = new Vector3(1, .5f, 1); // ������ ũ�� ����

        stackItems.Add(item);

        UpdateStackPositions();
    }
    
    public Transform PopItem()
    {
        if (stackItems.Count == 0) return null;
        
        Transform topItem = stackItems[stackItems.Count - 1];
        stackItems.RemoveAt(stackItems.Count - 1);

        UpdateStackPositions();

        return topItem;
    }

    /// <summary>
    /// ���� ������ ������ ���� ���ÿ� �ִ� ��� ������Ʈ�� ��ġ�� �����մϴ�
    /// </summary>
    void UpdateStackPositions()
    {
        for (int i = 0; i < stackItems.Count; i++)
        {
            Vector3 targetPosition = new Vector3(0, i * stackSpacing, 0);
            stackItems[i].localPosition = targetPosition;
        }
    }
}

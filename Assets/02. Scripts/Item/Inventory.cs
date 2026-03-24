using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int count = 0; // ������ ������ ��
    public int maxCount = 10; // �ִ� ���� ������ ������ ��
    public bool IsFull => Count >= maxCount; // �κ��丮�� ���� á���� ����

    // ���� Ȯ�强 ���(UI ������Ʈ ��)�Ͽ� ������Ƽ�� ����
    public int Count
    {
        get => count;
        private set
        {
            count = Mathf.Clamp(value, 0, maxCount); // Count�� 0�� maxCount ���̿� �ֵ��� ����
        }
    }

    public void Add(int _amount)
    {
        Count += _amount;

    }

    public void Remove(int _amount)
    {
        Count -= _amount;

    }
}

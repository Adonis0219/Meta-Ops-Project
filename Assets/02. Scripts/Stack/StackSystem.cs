using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackSystem : MonoBehaviour
{
    public float stackSpacing = 1.0f;
    public GameObject handObj;

    private Dictionary<PoolObejectType, List<Transform>> itemDict = new Dictionary<PoolObejectType, List<Transform>>();

    public Transform[] roots;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        handObj.SetActive(roots[(int)PoolObejectType.Product].childCount != 0);
    }

    void Init()
    {
        for (int i = 0; i < (int)PoolObejectType.Length; i++)
            itemDict[(PoolObejectType)i] = new List<Transform>();
    }

    public void AddToStack(Item item)
    {
        Transform itemTrans = item.transform;

        itemTrans.SetParent(roots[(int)item.poolType], false);

        itemTrans.localPosition = Vector3.zero; // 부모의 위치에 맞춰 초기 위치 설정
        itemTrans.localRotation = Quaternion.
            Euler(item.obtainAngle.x, item.obtainAngle.y, item.obtainAngle.z);  // 아이템의 획득 회전에 맞게 수정
        itemTrans.localScale = item.obtainScale; // 아이템의 크기를 수정

        itemDict[item.poolType].Add(itemTrans); 
        
        UpdateAllPos();
    }

    /// <summary>
    /// Removes and returns the top item from the stack.
    /// </summary>
    /// <param name="type">꺼내올 오브젝트의 <see cref="PoolObejectType"/></param>
    /// <returns>가장 상단 오브젝트의 <see cref="Transform"/>, 또는 스택이 비어있다면 <see langword="null"/> 반환</returns>
    public Transform PopItem(PoolObejectType type)
    {
        var list = itemDict[type];

        if (list.Count == 0) return null;

        Transform popItem = list[itemDict[type].Count - 1];
        list.RemoveAt(itemDict[type].Count - 1);

        UpdateAllPos();

        return popItem;
    }

    void UpdateAllPos()
    {
        foreach (var item in itemDict)
        {
            PoolObejectType type = item.Key;
            var list = item.Value;

            for (int j = 0; j < list.Count; j++)
            {
                Vector3 targetPos = new Vector3(0, j * stackSpacing, 0);
                list[j].localPosition = targetPos;
            }
        }
    }
}

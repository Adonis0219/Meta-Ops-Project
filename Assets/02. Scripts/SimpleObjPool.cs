using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObjPool : MonoBehaviour
{
    [Header("# Pool Settings")]
    public GameObject prefab;   // 풀링할 오브젝트
    public int initCount = 10;  // 초기 풀링 개수

    Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        for (int i = 0; i < initCount; i++)
        {
            GameObject obj = CreateObject();
            pool.Enqueue(obj);
        }
    }

    GameObject CreateObject()
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.SetActive(false);
        return obj;
    }

    public GameObject GetPool()
    {
        GameObject obj;

        if (pool.Count == 0)
            obj = CreateObject();
        else
            obj = pool.Dequeue();

        obj.SetActive(true);
        return obj;
    }

    public void ReturnPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject prefab;
    public QueueManager queueManager;

    public Transform spawnPoint;

    public int maxCount = 4;

    bool isSpawning = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn();
        }
    }

    void TrySpawn()
    {
        if (isSpawning) return;

        // 최대 인원 제한
        if (queueManager.Count >= maxCount) return;

        Customer last = queueManager.GetLastCusomer();

        // 첫 생성
        if (last == null)
        {
            Spawn();
            return;
        }

        // 마지막 손님이 도착했을 때만 생성
        if (last.IsArrived)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        isSpawning = true;

        GameObject obj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        Customer customer = obj.GetComponent<Customer>();

        customer.Init(queueManager);

        isSpawning = false;
    }
}

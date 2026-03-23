using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    Inventory playerInven;
    StackSystem stackSystem;

    private const string COLLECTIBLE_TAG = "Collectible";

    private void Awake()
    {
        playerInven = GetComponent<Inventory>();
        stackSystem = GetComponent<StackSystem>();

        if (playerInven == null)
            Debug.LogError("Inventory 컴포넌트가 없습니다!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(COLLECTIBLE_TAG))
        {
            Item item = other.GetComponent<Item>();

            if (item == null || item.isObtained) return;

            // 아이템 획득 처리
            item.isObtained = true;
            // 물리적 안정성을 위해 collider 비활성화
            Collider col = item.GetComponent<Collider>();
            if (col != null)
                col.enabled = false;

            if (playerInven.IsFull)
            {
                item.gameObject.SetActive(false); // 인벤이 가득 찼을 때 아이템 비활성화
                return;
            }

            // 인벤에 획득 알리기
            playerInven.Add(item.scoreValue);
            // 스택 시스템에 추가
            stackSystem.AddToStack(item.transform);
        }
    }
}
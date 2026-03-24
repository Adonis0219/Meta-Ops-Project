using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    Inventory playerInven;
    StackSystem stackSystem;

    [SerializeField] private float collectCooldown = 0.5f; // 수집 쿨다운 시간
    private float lastCollectTime = -Mathf.Infinity; // 마지막 수집 시간

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
            if (Time.time - lastCollectTime < collectCooldown)
                return; // 쿨다운 중이면 수집하지 않음

            // 마지막 수집 시간 업데이트
            lastCollectTime = Time.time;

            Item item = other.GetComponent<Item>();

            if (item == null || item.isObtained) return;

            // 아이템 획득 처리
            item.isObtained = true;
            // 물리적 안정성을 위해 collider 비활성화
            Collider col = item.GetComponent<Collider>();
            if (col != null)
                col.enabled = false;

            // 아이템 이동 알리기
            item.NotifyMoved();

            if (playerInven.IsFull)
            {
                item.gameObject.SetActive(false); // 인벤이 가득 찼을 때 아이템 비활성화
                return;
            }

            // 아이템 획득 효과
            ItemEffect effect = item.GetComponent<ItemEffect>();

            // 이동 효과
            StartCoroutine(effect.PlayCollectEffect(stackSystem.stackRoot));

            // 인벤에 획득 알리기
            playerInven.Add(item.scoreValue);
            // 스택 시스템에 추가
            stackSystem.AddToStack(item.transform);
        }
    }
}
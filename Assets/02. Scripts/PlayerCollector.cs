using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    Inventory playerInven;

    private const string COLLECTIBLE_TAG = "Collectible";

    private void Awake()
    {
        playerInven = GetComponent<Inventory>();

        if (playerInven == null)
            Debug.LogError("Inventory 컴포넌트가 없습니다!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(COLLECTIBLE_TAG))
        {
            Item item = other.GetComponent<Item>();

            if (item == null) return;

            // 점수 가져오기
            int score = item.scoreValue;
            // 인벤에 획득 알리기
            playerInven.Add(score);
            // 수집 아이템을 비활성화하여 수집된 것처럼 보이게 함
            // 후에 생성할 때 다시 활성화 가능
            other.gameObject.SetActive(false);   
        }
    }
}
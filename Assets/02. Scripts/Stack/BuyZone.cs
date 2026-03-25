using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BuyZone : BaseZone
{
    [Header("# Cost")]
    public int cost = 20;
    public float payDelay = .1f;

    [Header("# UI")]
    public TextMeshPro costText;
    public Vector3 offset = new Vector3(0, 2, 0);

    PlayerCollector player;

    public GameObject productPrefab;

    Coroutine payCoru;

    int paidCost = 0;

    private void Start()
    {
        UpdateCostUI();
    }

    private void LateUpdate()
    {
        // 텍스트 위치 고정
        costText.transform.position = transform.position + offset;

        // 카메라 바라보기
        costText.transform.forward = Camera.main.transform.forward;

        UpdateCostUI();
    }

    #region === Trigger ===

    protected override void OnEnterZone(Collider other)
    {
        if (payCoru != null) return;

        player = other.GetComponent<PlayerCollector>();

        var inven = other.GetComponent<Inventory>();
        var stack = other.GetComponent<StackSystem>();

        payCoru = StartCoroutine(PayRoutine(inven, stack));
    }

    protected override void OnExitZone(Collider other)
    {
        if (payCoru != null)
        {
            StopCoroutine(payCoru);
            payCoru = null;
        }
    }

    #endregion

    #region === Payment ===

    IEnumerator PayRoutine(Inventory inven, StackSystem stack)
    {
        while (paidCost < cost)
        {
            int count = inven.GetSlot(PoolObejectType.Money).Count;

            if (count <= 0)
                yield break;

            // 스택에서 돈 하나 꺼냄
            Transform money = stack.PopItem(PoolObejectType.Money);

            if (money == null) yield break;

            // 오브젝트 끄면 자동 반환
            money.gameObject.SetActive(false);

            inven.RemoveItem(PoolObejectType.Money, 1);

            paidCost += 10;

            yield return new WaitForSeconds(payDelay);
        }

        if (paidCost >= cost)
        {
            OnPurchaseComplete();
            paidCost = 0;
        }
    }

    void UpdateCostUI()
    {
        int remain = Mathf.Max(0, cost - paidCost);

        costText.text = $"{remain}";
    }

    void OnPurchaseComplete()
    {
        // 드릴 활성화
        player.ActiveDrill();
        // 발판 끄기
        gameObject.SetActive(false);
    }

    #endregion
}

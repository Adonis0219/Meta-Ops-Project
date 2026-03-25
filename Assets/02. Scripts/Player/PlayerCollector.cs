using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerCollector : MonoBehaviour
{
    Inventory playerInven;
    StackSystem stackSystem;

    [SerializeField] private float collectCooldown = 0.5f; // 채광 쿨다운 시간
    private float lastCollectTime = -Mathf.Infinity; // 마지막 수집 시간

    public GameObject drill;
    public BoxCollider drillCol;
    public CapsuleCollider playerCol;

    Coroutine proCoru, moneyCoru;

    private const string COLLECTABLE_TAG = "Collectable";
    private const string PRODUCTZONE_TAG = "ProductZone";
    private const string MONEYZONE_TAG = "MoneyZone";

    private void Awake()
    {
        playerInven = GetComponent<Inventory>();
        stackSystem = GetComponent<StackSystem>();

        if (playerInven == null)
            Debug.LogError("Inventory 컴포넌트가 없습니다!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(COLLECTABLE_TAG))
        {
            OreCollect(other);
        }
        else if (other.CompareTag(PRODUCTZONE_TAG))
        {
            proCoru = StartCoroutine(ProductCollect(other));
        }
        else if (other.CompareTag(MONEYZONE_TAG))
        {
            moneyCoru = StartCoroutine(MoneyCollect(other));
        }
    }
    
    public void ActiveDrill()
    {
        drill.SetActive(true);
        drillCol.enabled = true;
        playerCol.enabled = false;
        collectCooldown = 0.05f;
        playerInven.maxCount[0] = 100;
    }

    private void OreCollect(Collider other)
    {
        if (Time.time - lastCollectTime < collectCooldown)
            return; // 쿨다운 중이면 수집하지 않음

        // 마지막 수집 시간 업데이트
        lastCollectTime = Time.time;

        Item item = other.GetComponent<Item>();

        if (item == null || item.isObtained) return;

        // 아이템 획득 처리
        MarkAsCollected(item);

        // 아이템 이동 알리기
        item.NotifyMoved();

        if (playerInven.GetSlot(item.poolType).IsFull)
        {
            item.gameObject.SetActive(false); // 인벤이 가득 찼을 때 아이템 비활성화
            return;
        }

        // 아이템 획득 효과
        ObtainEffect(item);
    }

    IEnumerator MoneyCollect(Collider other)
    {
        MoneyZone mZone = other.GetComponent<MoneyZone>();

        while (mZone.moneyCount > 0)
        {
            Item money = mZone.MoneyObjects[^1].GetComponent<Item>();

            if (money == null || money.isObtained) yield break;

            mZone.Remove(money.GetComponent<MoneyObject>());

            // 아이템 획득 처리
            MarkAsCollected(money);

            // 아이템 획득 효과
            ObtainEffect(money);

            yield return new WaitForSeconds(.1f);
        }

        moneyCoru = null;
    }

    IEnumerator ProductCollect(Collider other)
    {
        ProductZone pZone = other.GetComponent<ProductZone>();

        while (pZone.ProdCount > 0)
        {
            // 가장 위 아이템 가져오기
            Item item = pZone.Products[pZone.ProdCount - 1].GetComponent<Item>();

            if (item == null || item.isObtained) yield break;

            // 아이템 지우기
            pZone.Remove(item.gameObject);

            // 아이템 획득 처리
            MarkAsCollected(item);

            // 아이템 획득 효과
            ObtainEffect(item);

            yield return new WaitForSeconds(.1f);
        }

        proCoru = null;
    }

    /// <summary>
    /// 아이템 획득 처리
    /// </summary>
    void MarkAsCollected(Item item)
    {
        // 아이템 획득 처리
        item.isObtained = true;

        // 물리적 안정성 위해 Collider 제거
        Collider col = item.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
    }

    void ObtainEffect(Item item)
    {
        // 아이템 획득 효과
        ItemEffect effect = item.GetComponent<ItemEffect>();

        // 이동 효과 시작
        //StartCoroutine(effect.PlayCollectEffect(stackSystem.roots[(int)item.poolType]));
        StartCoroutine(CollectRoutine(item));

        // 인벤에 획득 알리기
        playerInven.AddItem(item.poolType, item.scoreValue);
        // 스택 시스템의 자기 자리에 추가
        //stackSystem.AddToStack(item);
    }

    IEnumerator CollectRoutine(Item item)
    {
        ItemEffect effect = item.GetComponent<ItemEffect>();

        yield return effect.PlayCollectEffect(stackSystem.roots[(int)item.poolType]);

        stackSystem.AddToStack(item);
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ItemSlot
{
    int count;
    public int maxCount; // 해당 아이템의 최대 수량

    public int Count => count; // 해당 아이템의 수량
    public bool IsFull => count >= maxCount; // 해당 아이템이 최대 수량에 도달했는지 여부

    public ItemSlot(int _maxCount)
    {
        maxCount = _maxCount;
    }

    public void Add(int _amount)
    {
        count = Mathf.Clamp(count + _amount, 0, maxCount);
    }

    public void Remove(int _amount)
    {
        count = Mathf.Clamp(count - _amount, 0, maxCount);
    }
}

public class Inventory : MonoBehaviour
{
    Dictionary<PoolObejectType, ItemSlot> items = new Dictionary<PoolObejectType, ItemSlot>();

    public TextMeshProUGUI moneyText;


    public int Money => items[PoolObejectType.Money].Count * 10;

    // 임시 관리
    public int[] maxCount = { 10, 100, 100, 100 };

    private void Awake()
    {
        DictInitSet();
    }

    private void DictInitSet()
    {
        for (int i = 0; i < (int)PoolObejectType.Length; i++)
            items[(PoolObejectType)i] = new ItemSlot(maxCount[i]);
    }

    #region === Item ===

    public void AddItem(PoolObejectType _type, int _amount)
    {
        GetOrCreateSlot(_type).Add(_amount);

        if (_type == PoolObejectType.Money)
            UpdateMoneyUI();
    }

    public void RemoveItem(PoolObejectType _type, int _amount)
    {
        GetOrCreateSlot(_type).Remove(_amount);

        if (_type == PoolObejectType.Money)
            UpdateMoneyUI();
    }

    /// <summary>
    /// Dictionary를 private으로 설정하기 위한 Getter
    /// </summary>
    /// <param name="_type">키값</param>
    /// <returns>반환할 슬롯</returns>
    /// <remarks><see cref="ItemSlot"/> 자료형으로 반환하여 더 다양한 내부 필드를 사용할 수 있게 함</remarks>
    public ItemSlot GetSlot(PoolObejectType _type)
    {
        return GetOrCreateSlot(_type);
    }

    /// <summary>
    /// 키 값인 <see langword="_type"/>에 해당하는 Slot의 존재 여부에 따라 반환해주는 함수
    /// </summary>
    /// <param name="_type">키 값</param>
    /// <returns>반환할 슬롯</returns>
    /// <remarks><see cref="DictInitSet()"/>로 인해 사용할 일은 없지만 후에 <see cref="PoolObejectType"/>값 추가 대비</remarks>
    private ItemSlot GetOrCreateSlot(PoolObejectType _type)
    {
        // 딕셔너리에 추가할 타입의 슬롯이 없다면
        if (!items.ContainsKey(_type))
            // 아이템 슬롯 생성 후 할당
            items[_type] = new ItemSlot((int)_type);

        return items[_type];
    }

    #endregion

    void UpdateMoneyUI()
    {
        moneyText.text = Money.ToString();
    }
}

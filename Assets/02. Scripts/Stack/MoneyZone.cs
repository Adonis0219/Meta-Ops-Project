using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MoneyZone : MonoBehaviour
{
    public int price = 10;
    public float moneySpacing = .21f;
    public float basicSpacing = .2f;

    public TextMeshProUGUI moneyText;
    public Transform counterPoint;

    List<MoneyObject> moneyObjects = new List<MoneyObject>();
    MoneyObject moneyObject;

    public IReadOnlyList<MoneyObject> MoneyObjects => moneyObjects;
    public int moneyCount => moneyObjects.Count;

    public void AddMoney()
    {
        // µ· ½×ÀÌ´Â ¿¬Ãâ
        // µ· »ý¼º
        GameObject temp = PoolManager.instance.GetPool(PoolObejectType.Money);

        moneyObject = temp.GetComponent<MoneyObject>();
        moneyObject.SetParent(temp.transform.parent);

        // µ· ÀÌµ¿ (CustomerÀ§Ä¡(CountPoint) -> MoneyZone)
        moneyObject.InitBehaviour(counterPoint.position, transform.position);
        // ¸®½ºÆ®¿¡ µ· Ãß°¡
        moneyObjects.Add(moneyObject);

        moneyObject.OnArrived += UpdateStackPosition;
    }

    void UpdateStackPosition()
    {
        for (int i = 0; i < moneyObjects.Count; i++)
        {
            Transform money = moneyObjects[i].transform;

            money.localPosition = transform.position + new Vector3(0, basicSpacing + i * moneySpacing,0);
        }

        // ±¸µ¶ ÇØÁ¦
        moneyObject.OnArrived -= UpdateStackPosition;
    }

    public void Remove(MoneyObject item)
    {
        moneyObjects.Remove(item);
    }
}

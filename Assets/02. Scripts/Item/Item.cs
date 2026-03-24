using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum OreType
{
    Iron,
    Gold,
    Diamond
}

public class Item : MonoBehaviour
{
    public event Action<int, int> OnMoved;

    int x, y;

    #region === Inspector ===

    public OreType oreType; // 아이템의 광석 종류
    public int scoreValue = 1; // 아이템이 제공하는 점수 값

    public bool isObtained = false; // 아이템이 획득되었는지 여부

    #endregion

    public void Init(int _x, int _y)
    {
        // this 생략 가능
        this.x = _x;
        this.y = _y;
    }

    // 아이템이 다른 위치로 이동할 때 호출되는 메서드
    public void NotifyMoved()
    {
        // 기존 위치 알림
        OnMoved?.Invoke(x, y);
    }

    public void UpdateGridPos(int newX, int newY)
    {
        x = newX;
        y = newY;
    }
}

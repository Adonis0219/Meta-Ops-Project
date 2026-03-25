using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Item : PoolObject
{
    public event Action<int, int> OnMoved;

    // 생성될 당시 부모의 트랜스폼
    public Transform parent;
    
    int x, y;

    #region === Inspector ===

    public int scoreValue = 1; // 아이템이 제공하는 점수 값

    public bool isObtained = false; // 아이템이 획득되었는지 여부

    public Vector3 obtainScale;
    public Vector3 obtainAngle;

    #endregion

    private void OnDisable()
    {
        // 아이템이 비활성화될 때 획득 상태 초기화
        isObtained = false;

        // 풀로 되돌리기
        ReturnPool();
    }

    public void Init(int _x, int _y, Transform _parent)
    {
        // this 생략 가능
        this.x = _x;
        this.y = _y;
        parent = _parent;
    }

    public void SetParent(Transform parent)
    {
        this.parent = parent;
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

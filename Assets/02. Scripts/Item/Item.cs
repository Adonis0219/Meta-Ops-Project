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

    [Header("# Item Info")]
    public int scoreValue = 1; // 아이템이 제공하는 점수 값

    public bool isObtained = false; // 아이템이 획득되었는지 여부

    [Header("# Transform")]
    public Vector3 obtainScale;
    public Vector3 obtainAngle;

    #endregion

    private void OnDisable()
    {
        // 아이템이 비활성화될 때 획득 상태 초기화
        isObtained = false;

        if (poolType == PoolObejectType.Ore)
        {
            // 광석은 풀에 들어갈 때 원래 크기로
            transform.localScale = Vector3.one;
        }

        // 풀로 되돌리기
        ReturnPool();
    }

    #region === Init ===

    public void Init(int _x, int _y, Transform _parent)
    {
        x = _x;
        y = _y;
        parent = _parent;
    }

    public void SetParent(Transform parent)
    {
        this.parent = parent;
    }

    #endregion

    #region === Grid ===

    public void NotifyMoved()
    {
        OnMoved?.Invoke(x, y);
    }

    #endregion
}

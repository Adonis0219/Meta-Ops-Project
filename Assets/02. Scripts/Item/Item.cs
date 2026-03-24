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
    public OreType oreType; // 아이템의 광석 종류
    public int scoreValue = 1; // 아이템이 제공하는 점수 값

    public bool isObtained = false; // 아이템이 획득되었는지 여부
}

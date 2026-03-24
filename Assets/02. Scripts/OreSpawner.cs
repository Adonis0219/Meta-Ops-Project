using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreSpawner : MonoBehaviour
{
    #region === Inspector ===

    [Header("# Grid Settings")]
    public int width = 5;
    public int height = 20;

    [Header("# Respawn")]
    public float respawnDelay = 3f;

    [Header("# Reference")]
    public SimpleObjPool orePool;

    #endregion

    #region === Private Fields ===

    GameObject[,] oreGrid;

    #endregion

    private void Start()
    {
        InitGridSet();
        InitSpawn();
    }

    #region === Init ===

    void InitGridSet()
    {
        oreGrid = new GameObject[width, height];    
    }

    void InitSpawn()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Spawn(i, j);
            }
        }
    }

    #endregion

    public void Spawn(int _x, int _y)
    {
        if (oreGrid[_x, _y] != null) return;

        GameObject ore  = orePool.GetPool();
        //ore.transform.SetParent(this.transform);
        ore.transform.position = transform.position 
            + new Vector3(_y + _x, 0, _y - _x);

        Item item = ore.GetComponent<Item>();
        item.Init(_x, _y);

        item.OnMoved += HandleItemMoved;

        oreGrid[_x, _y] = ore;  
    }

    void HandleItemMoved(int _x, int _y)
    {
        if (oreGrid[_x, _y] == null) return;

        // 이벤트 해제 (중요함!!)
        Item item = oreGrid[_x, _y].GetComponent<Item>();
        item.OnMoved -= HandleItemMoved;

        // 해당 위치 비우기
        oreGrid[_x, _y] = null;

        // 일정 시간 후에 재스폰
        StartCoroutine(Respawn(_x, _y));
    }

    IEnumerator Respawn(int _x, int _y)
    {
        yield return new WaitForSeconds(respawnDelay);
        Spawn(_x, _y);
    }
}


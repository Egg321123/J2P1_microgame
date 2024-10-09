using System;
using UnityEngine;

public class MonoTowerTile : MonoTile
{
    private TowerData TowerData => Data.towerData ?? throw new NullReferenceException();

    private void Start()
    {
        Instantiate(TowerData.tower, transform.position, Quaternion.identity);
    }
}

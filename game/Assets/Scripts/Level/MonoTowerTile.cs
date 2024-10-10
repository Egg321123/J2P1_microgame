using System;
using UnityEngine;

public class MonoTowerTile : MonoTile
{
    private TowerData TowerData => Data.towerData ?? throw new NullReferenceException();

    private void Start()
    {
        MonoTile tower = Instantiate(TowerData.tower, transform.position, Quaternion.identity);
        tower.GetComponent<TowerBase>().TowerData = TowerData;
        tower.transform.parent = transform;
    }
}

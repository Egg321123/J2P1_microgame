using System;
using UnityEngine;

public class MonoTowerTile : MonoTile
{

    [SerializeField] private GameObject[] towerPrefabs;

    private TowerData TowerData => Data.towerData ?? throw new NullReferenceException();

    private void Start()
    {
        GameObject tower = Instantiate(GetTowerPrefab(), transform.position, Quaternion.identity);
        tower.GetComponent<TowerBase>().TowerData = TowerData;
        tower.transform.parent = transform;
    }

    private GameObject GetTowerPrefab()
    {
        foreach (GameObject tower in towerPrefabs)
            if (tower.name == TowerData.towerPrefabName) return tower;
        return null;
    }
}


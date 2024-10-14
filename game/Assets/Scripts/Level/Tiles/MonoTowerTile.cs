using System;
using UnityEngine;

public class MonoTowerTile : MonoTile
{
    // update this list if you add new towers to the game
    [Header("Towers")]
    [SerializeField] private GameObject ArrowTower;
    [SerializeField] private GameObject CannonTower;
    [SerializeField] private GameObject MagicTower;
    [SerializeField] private GameObject ShockwaveTower;

    private TowerData TowerData => Data.towerData ?? throw new NullReferenceException();

    private void Start()
    {
        GameObject tower = Instantiate(GetTowerPrefab(), transform.position, Quaternion.identity);
        tower.GetComponent<TowerBase>().TowerData = TowerData;
        tower.transform.parent = transform;
    }

    private GameObject GetTowerPrefab()
    {
        // update this list if you add new towers to the game
        return TowerData.towerType switch
        {
            TowerType.ArrowTower => ArrowTower,
            TowerType.CannonTower => CannonTower,
            TowerType.MagicTower => MagicTower,
            TowerType.ShockwaveTower => ShockwaveTower,
            _ => ArrowTower,
        };
    }
}

// update this list if you add new towers to the game
public enum TowerType
{
    ArrowTower,
    CannonTower,
    MagicTower,
    ShockwaveTower
}


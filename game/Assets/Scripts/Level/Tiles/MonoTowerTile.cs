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

    private TowerData TowerData => Data.towerData;

    protected override void Initialize()
    {
        GameObject tower = Instantiate(GetTowerPrefab(), transform.position, Quaternion.identity);
        tower.transform.parent = transform;
        tower.GetComponent<TowerBase>().TowerData = TowerData;
    }

    private GameObject GetTowerPrefab()
    {
        // update this list if you add new towers to the game
        return TowerData.towerType switch
        {
            TowerType.ARROW_TOWER => ArrowTower,
            TowerType.CANNON_TOWER => CannonTower,
            TowerType.MAGIC_TOWER => MagicTower,
            TowerType.SHOCKWAVE_TOWER => ShockwaveTower,
            _ => throw new IndexOutOfRangeException($"didn't recognise the tower: {TowerData.towerType}!"),
        };
    }
}

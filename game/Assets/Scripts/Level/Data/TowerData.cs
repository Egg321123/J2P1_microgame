using System;
using UnityEngine;

[Serializable]
public struct TowerData
{
    public string towerPrefabName;

    public float attackSpeed;
    public float attackRange;
    public float attackDamage;
    public float projectileSpeed;
    public int level;

    public TowerData(string towerPrefabName, float attackSpeed, float attackRange, float attackDamage, float projectileSpeed, int level = 1)
    {
        this.towerPrefabName = towerPrefabName;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.attackDamage = attackDamage;
        this.projectileSpeed = projectileSpeed;
        this.level = level;
    }
}

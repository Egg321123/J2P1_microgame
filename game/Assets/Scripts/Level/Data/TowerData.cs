using System;
using UnityEngine;

[Serializable]
public struct TowerData
{
    public MonoTile tower;

    public float attackSpeed;
    public float attackRange;
    public float attackDamage;
    public float projectileSpeed;
    public int level;

    public TowerData(MonoTile tower, float attackSpeed, float attackRange, float attackDamage, float projectileSpeed, int level = 1)
    {
        this.tower = tower;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.attackDamage = attackDamage;
        this.projectileSpeed = projectileSpeed;
        this.level = level;
    }
}

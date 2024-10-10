using UnityEngine;

public struct TowerData
{
    public GameObject tower;

    public float attackSpeed;
    public float attackRange;
    public float attackDamage;
    public float projectileSpeed;
    public int level;

    public TowerData(GameObject tower, float attackSpeed, float attackRange, float attackDamage, float projectileSpeed, int level = 1)
    {
        this.tower = tower;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.attackDamage = attackDamage;
        this.projectileSpeed = projectileSpeed;
        this.level = level;
    }
}

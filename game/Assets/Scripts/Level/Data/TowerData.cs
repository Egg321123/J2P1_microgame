using System;

[Serializable]
public struct TowerData
{
    public TowerType towerType;

    public float attackSpeed;
    public float attackRange;
    public int attackDamage;
    public float projectileSpeed;
    public int level;

    public TowerData(TowerType towerType, float attackSpeed, float attackRange, int attackDamage, float projectileSpeed, int level = 1)
    {
        this.towerType = towerType;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.attackDamage = attackDamage;
        this.projectileSpeed = projectileSpeed;
        this.level = level;
    }
}

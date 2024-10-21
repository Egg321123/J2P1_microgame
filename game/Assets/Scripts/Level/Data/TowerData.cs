using System;

[Serializable]
public struct TowerData
{
    public TowerType towerType;

    public float attackSpeed;
    public float attackRadius;
    public int attackDamage;
    public int targetCount;         // <0: select all targets
    public float projectileSpeed;
    public int level;

    public TowerData(TowerType towerType, float attackSpeed, float attackRadius, int attackDamage, int targetCount, float projectileSpeed, int level = 1)
    {
        this.towerType = towerType;
        this.attackSpeed = attackSpeed;
        this.attackRadius = attackRadius;
        this.attackDamage = attackDamage;
        this.targetCount = targetCount;
        this.projectileSpeed = projectileSpeed;
        this.level = level;
    }
}

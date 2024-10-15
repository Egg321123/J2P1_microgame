using System.Collections.Generic;
using UnityEngine;

public class ShockwaveTower : TowerBase
{
    protected override IEnumerable<EnemyBase> SelectTargets() => GameManager.Instance.Waves.GetEnemiesInRadius(transform.position,towerData.attackRange, -1);

    protected override void ShotTarget(EnemyBase target)
    {
        EnemyBase targetScript = target.GetComponent<EnemyBase>();
        targetScript.ApplyStun(2);
        targetScript.TakeDamage(towerData.attackDamage);
    }
}

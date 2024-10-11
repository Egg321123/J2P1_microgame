using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShockwaveTower : TowerBase
{
    protected override IEnumerable<GameObject> SelectTargets() => GameManager.Instance.Waves.GetEnemiesInRadius(transform.position,towerData.attackRange, -1);

    protected override void ShotTarget(GameObject target)
    {
        target.GetComponent<AIDeath>().Die();
    }
}

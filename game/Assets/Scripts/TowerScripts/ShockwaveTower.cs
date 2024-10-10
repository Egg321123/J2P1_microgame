using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShockwaveTower : TowerBase
{
    protected override List<GameObject> SelectTargets() => GameObjectUtils.GetAllOnLayer(gameObject, enemyMask, towerData.attackRange);

    protected override void ShotTarget(GameObject target)
    {
        target.GetComponent<AIDeath>().Die();
    }
}

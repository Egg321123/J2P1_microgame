using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : ProjectileTowerBase
{
    [SerializeField] private GameObject projectile;

    protected override List<GameObject> SelectTargets() => GameObjectUtils.GetNearestOnLayer(gameObject, enemyMask, towerData.attackRange, 1);

    protected override void ProjectileHit(GameObject target) => target.GetComponent<AIDeath>().Die();

    protected override void ShotTarget(GameObject target)
    {
        //create new trail
        GameObject trail = Instantiate(projectile, firingPoint.position, Quaternion.identity);
        trail.transform.parent = transform;
        trail.GetComponent<Projectile>().Initialize(firingPoint.position, target.transform, towerData.projectileSpeed);

        base.ShotTarget(target);
    }
}

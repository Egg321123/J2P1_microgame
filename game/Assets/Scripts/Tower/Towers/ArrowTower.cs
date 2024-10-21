using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : ProjectileTowerBase
{
    [SerializeField] private GameObject projectile;

    protected override IEnumerable<EnemyBase> SelectTargets() => GameManager.Instance.Waves.GetEnemiesInRadius(transform.position, TowerData.attackRange, 3);

    protected override void ProjectileHit(EnemyBase target) => target.GetComponent<EnemyBase>().TakeDamage(TowerData.attackDamage);

    protected override void ShotTarget(EnemyBase target)
    {
        //create new trail
        GameObject trail = Instantiate(projectile, firingPoint.position, Quaternion.identity);
        trail.transform.parent = transform;
        trail.GetComponent<TrailProjectile>().Initialize(firingPoint.position, target.transform, TowerData.projectileSpeed);

        base.ShotTarget(target);
    }
}

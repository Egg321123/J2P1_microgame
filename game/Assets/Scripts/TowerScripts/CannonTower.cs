using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower : ProjectileTowerBase
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject explosion;
    [SerializeField] private float explosionSize = 1;

    protected override IEnumerable<GameObject> SelectTargets() => GameManager.Instance.Waves.GetEnemiesInRadius(transform.position,towerData.attackRange, 1);

    protected override void ShotTarget(GameObject target)
    {
        //create new trail
        GameObject trail = Instantiate(projectile, firingPoint.position, Quaternion.identity);
        trail.transform.parent = transform;
        trail.GetComponent<Projectile>().Initialize(firingPoint.position, target.transform, towerData.projectileSpeed);
        Collider[] exploded = Physics.OverlapSphere(target.transform.position, explosionSize, enemyMask);

        base.ShotTarget(target);
    }

    protected override void ProjectileHit(GameObject target)
    {
        print("explosion");
        Collider[] exploded = Physics.OverlapSphere(target.transform.position, explosionSize, enemyMask);
        Instantiate(explosion, target.transform);
    }
}

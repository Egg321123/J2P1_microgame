using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CannonTower : ProjectileTowerBase
{
    [SerializeField] private GameObject audioPrefab;
    [SerializeField] private AudioClip clip;

    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject explosion;
    [SerializeField] private float explosionSize = 1;

    protected override IEnumerable<EnemyBase> SelectTargets() => Waves.GetEnemiesInRadius(transform.position,TowerData.attackRange, 1);

    protected override void ShotTarget(EnemyBase target)
    {
        //create new trail
        GameObject sound = Instantiate(audioPrefab, firingPoint.position, Quaternion.identity);
        sound.GetComponent<AudioClipPlayer>().Initialize(clip);

        GameObject trail = Instantiate(projectile, firingPoint.position, Quaternion.identity);
        trail.transform.parent = transform;
        trail.GetComponent<TrailProjectile>().Initialize(firingPoint.position, target.transform, TowerData.projectileSpeed);

        base.ShotTarget(target);
    }

    protected override void ProjectileHit(EnemyBase target)
    {
        print("explosion");
        EnemyBase[] objects = Waves.GetEnemiesInRadius(transform.position, TowerData.attackRange, -1).ToArray();
        Instantiate(explosion, target.transform);
        foreach (EnemyBase exploded in objects)
        {
            exploded.GetComponent<EnemyBase>().TakeDamage(TowerData.attackDamage);
        }
    }
}

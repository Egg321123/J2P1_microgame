using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : ProjectileTowerBase
{
    [SerializeField] private GameObject audioPrefab;
    [SerializeField] private AudioClip clip;
    [SerializeField] private GameObject projectile;

    protected override IEnumerable<EnemyBase> SelectTargets() => GameManager.Instance.Waves.GetEnemiesInRadius(transform.position, TowerData.attackRange, 1);

    protected override void ProjectileHit(EnemyBase target) => target.GetComponent<EnemyBase>().TakeDamage(TowerData.attackDamage);

    protected override void ShotTarget(EnemyBase target)
    {
        GameObject sound = Instantiate(audioPrefab, firingPoint.position, Quaternion.identity);
        sound.GetComponent<AudioClipPlayer>().Initialize(clip);
        //create new trail
        GameObject trail = Instantiate(projectile, firingPoint.position, Quaternion.identity);
        trail.transform.parent = transform;
        trail.GetComponent<TrailProjectile>().Initialize(firingPoint.position, target.transform, TowerData.projectileSpeed);

        base.ShotTarget(target);
    }
}

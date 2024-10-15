using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : ProjectileTowerBase
{
    [SerializeField] private GameObject audioPrefab;
    [SerializeField] private AudioClip clip;
    [SerializeField] private GameObject projectile;

    protected override IEnumerable<GameObject> SelectTargets() => GameManager.Instance.Waves.GetEnemiesInRadius(transform.position,towerData.attackRange, 1);

    protected override void ProjectileHit(GameObject target) => target.GetComponent<EnemyBase>().TakeDamage(towerData.attackDamage);

    protected override void ShotTarget(GameObject target)
    {
        GameObject sound = Instantiate(audioPrefab, firingPoint.position, Quaternion.identity);
        sound.GetComponent<AudioClipPlayer>().Initialize(clip);
        //create new trail
        GameObject trail = Instantiate(projectile, firingPoint.position, Quaternion.identity);
        trail.transform.parent = transform;
        trail.GetComponent<TrailProjectile>().Initialize(firingPoint.position, target.transform, towerData.projectileSpeed);

        base.ShotTarget(target);
    }
}

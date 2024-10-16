using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MagicTower : ProjectileTowerBase
{
    [SerializeField] private GameObject projectile;
    [SerializeField] GameObject animatedComponent;

    protected override void FixedUpdate()
    {
        animatedComponent.transform.Rotate(0, 30 * Time.deltaTime, 0);
        Vector3 anim = Vector3.zero;
        //anim.y = Mathf.Sin(Time.deltaTime * 10) * 100;
        animatedComponent.transform.localPosition = anim;

        base.FixedUpdate();
    }

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

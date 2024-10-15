using System.Collections;
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

    protected override IEnumerable<GameObject> SelectTargets() => GameManager.Instance.Waves.GetEnemiesInRadius(transform.position,towerData.attackRange, 1);

    protected override void ShotTarget(GameObject target)
    {
        //create new trail
        GameObject sound = Instantiate(audioPrefab, firingPoint.position, Quaternion.identity);
        sound.GetComponent<AudioClipPlayer>().Initialize(clip);

        GameObject trail = Instantiate(projectile, firingPoint.position, Quaternion.identity);
        trail.transform.parent = transform;
        trail.GetComponent<TrailProjectile>().Initialize(firingPoint.position, target.transform, towerData.projectileSpeed);

        base.ShotTarget(target);
    }

    protected override void ProjectileHit(GameObject target)
    {
        print("explosion");
        GameObject[] objects = GameManager.Instance.Waves.GetEnemiesInRadius(transform.position, towerData.attackRange, 0).ToArray();
        Instantiate(explosion, target.transform);
        foreach (GameObject exploded in objects)
        {
            exploded.GetComponent<EnemyBase>().TakeDamage(towerData.attackDamage);
        }
    }
}
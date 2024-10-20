using System.Collections;
using UnityEngine;

public abstract class ProjectileTowerBase : TowerBase
{
    // implementation for when the projectile has hit the target
    protected abstract void ProjectileHit(EnemyBase target);

    private IEnumerator AwaitProjectileHit(EnemyBase target)
    {
        //wait until the projectile is "done"
        yield return new WaitForSeconds(1 / TowerData.projectileSpeed);

        //check again if it's a valid object, due to delay
        if (!target.gameObject.activeInHierarchy) yield return null;
        else ProjectileHit(target);

        yield return null;
    }

    protected override void ShotTarget(EnemyBase target)
    {
        StartCoroutine(AwaitProjectileHit(target));
    }
}

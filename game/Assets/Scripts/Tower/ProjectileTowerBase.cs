using System.Collections;
using UnityEngine;

public abstract class ProjectileTowerBase : TowerBase
{
    protected abstract void ProjectileHit(GameObject target);

    private IEnumerator AwaitProjectileHit(GameObject target)
    {
        //wait until the projectile is "done"
        yield return new WaitForSeconds(1 / towerData.projectileSpeed);

        //check again if it's a valid object, due to delay
        if (!target.activeInHierarchy) yield return null;
        else ProjectileHit(target);

        yield return null;
    }

    protected override void ShotTarget(GameObject target)
    {
        StartCoroutine(AwaitProjectileHit(target));
    }
}

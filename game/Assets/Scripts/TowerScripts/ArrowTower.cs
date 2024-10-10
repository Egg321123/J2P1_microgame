using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : MonoTower
{
    [SerializeField] private GameObject projectile;

    protected override List<GameObject> SelectTargets() => FindNearestNthTargets(1);

    protected override void ShotTarget(GameObject target)
    {
        //create new trail
        GameObject trail = Instantiate(projectile, firingPoint.position, Quaternion.identity);
        trail.transform.parent = transform;
        trail.GetComponent<Projectile>().Initialize(firingPoint.position, target.transform, towerData.projectileSpeed);

        StartCoroutine(AwaitProjectileHit(target));
    }

    private IEnumerator AwaitProjectileHit(GameObject target)
    {
        //wait until the projectile is "done"
        yield return new WaitForSeconds(1 / towerData.projectileSpeed);

        //check again if it's a valid object, due to delay
        if (!target.activeInHierarchy) yield return null;
        else target.GetComponent<AIDeath>().Die();

        yield return null;
    }
}

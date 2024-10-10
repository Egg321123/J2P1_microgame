using System.Collections;
using UnityEngine;

public class ArrowTower : MonoTower
{
    [SerializeField] private GameObject projectile;

    private GameObject target = null;
    private bool isAllowedToShoot = true;

    private void Start() => StartCoroutine(ShootLoop());


    //only try finding target every fixed updated (for fewer updates)
    private void FixedUpdate() => target = FindNearestTarget();

    protected IEnumerator ShootLoop()
    {
        while (isAllowedToShoot)
        {
            //wait for shooting delay
            yield return new WaitForSeconds(1 / towerData.attackSpeed);

            //trigger the shooting behavior if the object is valid, otherwise wait for next frame
            if (target == null || !target.activeInHierarchy) yield return null;
            else Shoot();

            //wait until the projectile is "done"
            yield return new WaitForSeconds(1 / towerData.projectileSpeed);

            //check again if it's a valid object, due to delay
            if (target == null || !target.activeInHierarchy) yield return null;
            else target.GetComponent<AIDeath>().Die();

            yield return null;
        }
        yield return null;
    }

    //runs when the parent script runs Shoot
    private void Shoot()
    {
        //create new trail
        GameObject trail = Instantiate(projectile, firingPoint.position, Quaternion.identity);
        trail.transform.parent = transform;
        trail.GetComponent<Projectile>().Initialize(firingPoint.position, target.transform, towerData.projectileSpeed);
    }

#if UNITY_EDITOR
    // draw path for debuggong
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, towerData.attackRange);

        Gizmos.color = Color.red;
        if (target != null) Gizmos.DrawSphere(target.transform.position + new Vector3(0, 1, 0), 0.1f);
    }
#endif
}

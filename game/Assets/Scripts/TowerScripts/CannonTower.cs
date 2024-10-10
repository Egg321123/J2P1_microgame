using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CannonTower : MonoTower
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject explosion;

    private GameObject target = null;

    [SerializeField] private float explosionSize = 1;
    private bool isAllowedToShoot = true;

    private void Start() => StartCoroutine(ShootLoop());


    //only try finding target every fixed updated (for fewer updates)
    private void FixedUpdate() => target = FindNearestTarget();

    protected IEnumerator ShootLoop()
    {
        print("start shoot loop");
        while (isAllowedToShoot)
        {
            print("allowed to shoot");
            //wait for shooting delay
            yield return new WaitForSeconds(1 / towerData.attackSpeed);
            print("shooting delay over");

            if (target == null || !target.activeInHierarchy) yield return null;
            else
            {
                print("shooting");
                Shoot(target);
            }
                

            //wait until the projectile is "done"
            yield return new WaitForSeconds(1 / towerData.projectileSpeed);

            if (target == null || !target.activeInHierarchy) yield return null;
            else
            {
                print("explosion");
                Collider[] exploded = Physics.OverlapSphere(target.transform.position, explosionSize, enemyMask);
                Instantiate(explosion, target.transform);
            }

            yield return null;
        }
        yield return null;
    }

    //runs when the parent script runs Shoot
    private void Shoot(GameObject target)
    {
        //create new trail
        GameObject trail = Instantiate(projectile, firingPoint.position, Quaternion.identity);
        trail.transform.parent = transform;
        trail.GetComponent<Projectile>().Initialize(firingPoint.position, target.transform, towerData.projectileSpeed);
        Collider[] exploded = Physics.OverlapSphere(target.transform.position, explosionSize, enemyMask);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, towerData.attackRange);

        Gizmos.color = Color.red;
        if (target != null)
        {
            Gizmos.DrawSphere(target.transform.position + new Vector3(0, 1, 0), 0.1f);
        }
    }
#endif
}

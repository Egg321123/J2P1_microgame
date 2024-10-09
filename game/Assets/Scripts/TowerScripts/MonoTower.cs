using System.Collections;
using System.IO;
using Unity.Mathematics;
using UnityEngine;


public abstract class MonoTower : MonoBehaviour
{

    [SerializeField] protected Transform firingPoint;
    [SerializeField] protected LayerMask enemyMask;
    protected Transform target;
    protected TowerData towerData;

    public TowerData TowerData { set { towerData = value; } }

    Coroutine shootLoop = null;

    // Update is called once per frame
    void Update()
    {
        //if target doesn't exist, find a new one
        if (target == null || !target.gameObject.activeInHierarchy) {

            //find new target
            FindTarget();

            return;
        }

        //if target is not in range, clear target
        if (!CheckIfTargetInRange()) {

            //if there is no target, stop shooting
            if (shootLoop != null)
            {
                StopCoroutine(shootLoop);
                shootLoop = null;
            }

            //clear target
            target = null;

            return;
        }

        //rest of logic is if there is a target, so start loop
        if (shootLoop == null) shootLoop = StartCoroutine(ShootLoop());    
    }

    IEnumerator ShootLoop()
    {
        while (target != null) {
            yield return new WaitForSeconds(towerData.attackSpeed);

            Shoot();

            yield return new WaitForSeconds(towerData.bulletSpeed);
            target.GetComponent<AIDeath>().Die();

            yield return null;
        }

        yield return null;
    }

    protected abstract void Shoot();

    private void FindTarget()
    {
        //first object that overlaps, make target
        Collider[] hits = Physics.OverlapSphere(transform.position, towerData.attackRange, enemyMask);

        Transform nearestTransform = null;
        float nearestDistance = math.INFINITY;

        if (hits.Length <= 0) return;
        foreach (Collider hit in hits)
        {
            Transform hitTransform = hit.transform;
            float distance = Vector3.Distance(transform.position, hitTransform.position);

            if (distance < nearestDistance)
            {
                nearestTransform = hitTransform;
                nearestDistance = distance;
            }
        }

        target = nearestTransform;
    }

    /// <summary>
    /// if enemy distance is less than or equal to the towers range
    /// </summary>
    /// <returns>returns a bool that will return true there are no enemies in range</returns>
    private bool CheckIfTargetInRange()
    {
        //check if target is still in range
        return Vector3.Distance(target.position, transform.position) < towerData.attackRange;
    }


#if UNITY_EDITOR
    // draw path for debuggong
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, towerData.attackRange);

        Gizmos.color = Color.red;
        if (target != null) Gizmos.DrawSphere(target.position + new Vector3(0, 1, 0), 0.1f);
    }
#endif
}

using System.Collections;
using UnityEngine;

public class shockwaveTower : MonoTower
{
    private GameObject[] targets = null;
    private bool isAllowedToShoot = true;

    private void Start() => StartCoroutine(ShootLoop());


    //only try finding target every fixed updated (for fewer updates)
    private void FixedUpdate() => targets = FindAllTargets();

    protected IEnumerator ShootLoop()
    {
        while (isAllowedToShoot)
        {
            //wait for shooting delay
            yield return new WaitForSeconds(1 / towerData.attackSpeed);

            foreach (GameObject target in targets)
            {
                if (target == null || !target.activeInHierarchy) yield return null;
                else target.GetComponent<AIDeath>().Die();
            }

            yield return null;
        }
        yield return null;
    }

#if UNITY_EDITOR
    // draw path for debuggong
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, towerData.attackRange);

        Gizmos.color = Color.red;
        if (targets != null)
        {
            foreach (GameObject target in targets)
            {
                Gizmos.DrawSphere(target.transform.position + new Vector3(0, 1, 0), 0.1f);
            }
        }
    }
#endif
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class TowerBase : MonoBehaviour
{
    [SerializeField] protected Transform firingPoint;
    [SerializeField] protected LayerMask enemyMask;
    protected TowerData towerData;
    private IEnumerable<EnemyBase> targets = null;
    private bool isAllowedToShoot = true;

    public TowerData TowerData { set { towerData = value; } }

    protected virtual void Start() => StartCoroutine(ShootLoop());
    protected virtual void FixedUpdate() => targets = SelectTargets(); //only try finding target every fixed updated (for fewer updates)

    protected abstract IEnumerable<EnemyBase> SelectTargets();
    protected abstract void ShotTarget(EnemyBase target);

    private IEnumerator ShootLoop()
    {
        while (isAllowedToShoot)
        {
            //wait for shooting delay
            yield return new WaitForSeconds(1 / towerData.attackSpeed);

            foreach (EnemyBase target in targets)
                ShotTarget(target);

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
            foreach (EnemyBase target in targets)
            {
                Gizmos.DrawSphere(target.transform.position + new Vector3(0, 1, 0), 0.1f);
            }
        }
    }
#endif
}

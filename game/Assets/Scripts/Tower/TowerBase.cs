using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class TowerBase : MonoBehaviour
{
    [SerializeField] protected Transform firingPoint;
    private TowerData towerData;                        // contains the data of this tower
    private IEnumerable<EnemyBase> targets = null;      // the enemies that are currently being targeted
    private bool isAllowedToShoot = true;               // whether the tower is allowed to shoot

    // shorthands and diverse properties
    public TowerData TowerData => towerData;
    protected GameManager GameManager => GameManager.Instance;
    protected Waves Waves => GameManager.Waves;

    // unity update functions
    protected virtual void Start() => StartCoroutine(ShootLoop());
    protected virtual void FixedUpdate() => targets = SelectTargets(); //only try finding target every fixed updated (for fewer updates)

    protected abstract IEnumerable<EnemyBase> SelectTargets();  // implementation for selecting targets
    protected abstract void ShotTarget(EnemyBase target);       // implementation for when a target has been shot

    // timer for shooting
    private IEnumerator ShootLoop()
    {
        while (isAllowedToShoot)
        {
            //wait for shooting delay
            yield return new WaitForSeconds(1 / TowerData.attackSpeed);

            foreach (EnemyBase target in targets)
                ShotTarget(target);

            yield return null;
        }
        yield return null;
    }

#if UNITY_EDITOR // debug utilities
    // draw path for debuggong
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, TowerData.attackRange);

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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class TowerBase : MonoBehaviour
{
    [SerializeField] protected Transform firingPoint;
    [SerializeField] protected GameObject audioPrefab;
    [SerializeField] protected AudioClip clip;

    private TowerData towerData;                        // contains the data of this tower
    private IEnumerable<EnemyBase> targets = null;      // the enemies that are currently being targeted
    private bool isAllowedToShoot = true;               // whether the tower is allowed to shoot

    // shorthands and diverse properties
    public TowerData TowerData { get => towerData; set => towerData = value; }
    protected GameManager GameManager => GameManager.Instance;
    protected Waves Waves => GameManager.Waves;

    // unity update functions
    protected virtual void Start() => StartCoroutine(ShootLoop());
    protected virtual void FixedUpdate() => targets = SelectTargets();  //only try finding target every fixed updated (for fewer updates)

    protected abstract void ShotTarget(EnemyBase target);               // implementation for when a target has been shot

    protected virtual void BeforeShootDelay() { }                       // implementation to get when it getting ready to shoot
    protected virtual void AfterShootDelay()                         // implementation to get when it getting ready to shoot
    {
        GameObject sound = Instantiate(audioPrefab, firingPoint.position, Quaternion.identity);
        sound.GetComponent<AudioClipPlayer>().Initialize(clip, false);
    }  


    // selects the targets that will be shot
    private IEnumerable<EnemyBase> SelectTargets()
    {
        // use the tower data to get the enemies within a radius of this tower
        return Waves.GetEnemiesInRadius(transform.position, towerData.attackRadius, towerData.targetCount);
    }

    // timer for shooting
    private IEnumerator ShootLoop()
    {
        while (isAllowedToShoot)
        {
            BeforeShootDelay();

            //wait for shooting delay
            yield return new WaitForSeconds(1 / TowerData.attackSpeed);

            if (targets.ToArray().Length > 0) AfterShootDelay();

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
        Gizmos.DrawWireSphere(transform.position, TowerData.attackRadius);

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

using UnityEngine;

public class MagicTower : ProjectileTowerBase
{
    [SerializeField] private GameObject projectile;
    [SerializeField] GameObject animatedComponent;
    private float towerTime = 0;

    protected override void FixedUpdate()
    {
        towerTime += Time.deltaTime;

        animatedComponent.transform.Rotate(0, 30 * Time.deltaTime, 0);
        Vector3 anim = Vector3.zero;
        anim.y = Mathf.Sin(towerTime * 1) * 0.2f;
        animatedComponent.transform.localPosition = anim;

        base.FixedUpdate();
    }

    protected override void ProjectileHit(EnemyBase target) => target.GetComponent<EnemyBase>().TakeDamage(TowerData.attackDamage);

    protected override void ShotTarget(EnemyBase target)
    {
        //create new trail
        GameObject trail = Instantiate(projectile, firingPoint.position, Quaternion.identity);
        trail.transform.parent = transform;
        trail.GetComponent<TrailProjectile>().Initialize(firingPoint.position, target.transform, TowerData.projectileSpeed);

        base.ShotTarget(target);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class ShockwaveTower : TowerBase
{
    protected override IEnumerable<GameObject> SelectTargets() => GameManager.Instance.Waves.GetEnemiesInRadius(transform.position,towerData.attackRange, -1);
    [SerializeField] private GameObject audioPrefab;
    [SerializeField] private AudioClip clip;

    protected override void ShotTarget(EnemyBase target)
    {
        GameObject sound = Instantiate(audioPrefab, firingPoint.position, Quaternion.identity);
        sound.GetComponent<AudioClipPlayer>().Initialize(clip);
        EnemyBase targetScript = target.GetComponent<EnemyBase>();
        targetScript.ApplyStun(2);
        targetScript.TakeDamage(towerData.attackDamage);
    }
}

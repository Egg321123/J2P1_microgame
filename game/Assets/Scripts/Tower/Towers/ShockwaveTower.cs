using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveTower : TowerBase
{
    //when in delay, move movingcomponent to 0.7 until delay is done, then go back to 0 quickly
    [SerializeField] GameObject animatedComponent;

    protected override IEnumerable<EnemyBase> SelectTargets() => Waves.GetEnemiesInRadius(transform.position, TowerData.attackRange, -1);

    protected override void BeforeShootDelay() => StartCoroutine(TowerAnimation());

    private IEnumerator TowerAnimation()
    {
        float currentTime = 0;
        float animTime = (1 / TowerData.attackSpeed) - 0.1f;

        while (currentTime < animTime)
        {
            currentTime += Time.deltaTime;
            float posInLerp = currentTime / animTime;

            animatedComponent.transform.localPosition = Vector3.Lerp(new(0,0,0), new(0,0.7f,0), posInLerp);
            
            yield return null;
        }

        currentTime = 0;
        animTime = 0.1f;

        while (currentTime < animTime)
        {
            currentTime += Time.deltaTime;
            float posInLerp = currentTime / animTime;

            animatedComponent.transform.localPosition = Vector3.Lerp(new(0, 0.7f, 0), new(0, 0, 0), posInLerp);

            yield return null;
        }

        yield return null;
    } 

    protected override void ShotTarget(EnemyBase target)
    {
        GameObject sound = Instantiate(audioPrefab, firingPoint.position, Quaternion.identity);
        sound.GetComponent<AudioClipPlayer>().Initialize(clip);
        EnemyBase targetScript = target.GetComponent<EnemyBase>();
        targetScript.ApplyStun(2);
        targetScript.TakeDamage(TowerData.attackDamage);
    }
}

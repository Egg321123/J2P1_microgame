using System.Collections;
using UnityEngine;

public class ArrowTower : MonoTower
{
    [SerializeField] TrailRenderer trailRenderer;

    //runs when the parent script runs Shoot
    protected override void Shoot()
    {
        //get the direction of the target
        Vector3 dir = (target.position - firingPoint.position).normalized;

        //display
        Debug.DrawRay(firingPoint.position, dir, Color.red, 1000f);

        //create new trail
        TrailRenderer trail = Instantiate(trailRenderer, firingPoint.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail));

    }

    private IEnumerator SpawnTrail(TrailRenderer trail)
    {
        //save data to request less
        float bulletSpeed = towerData.bulletSpeed;

        //get the start position
        Vector3 startPosition = trail.transform.position;

        //lerp to target position
        float currentTime = 0;
        while (currentTime < bulletSpeed)
        {
            //if the target no longer exists, please stop
            if (target == null) break;

            float positionAlongLerp = currentTime / bulletSpeed;

            //update position
            trail.transform.position = Vector3.Lerp(startPosition, target.position, positionAlongLerp);
            currentTime += Time.deltaTime;

            //wait for next frame
            yield return null;
        }

        //destroy the trail
        Destroy(trail.gameObject);

        //wait for next frame
        yield return null;
    }
}

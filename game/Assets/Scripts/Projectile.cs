using System.Collections;
using UnityEngine;
public class Projectile : MonoBehaviour
{
    Vector3 origin;
    Transform target;
    float projectileSpeed;

    public void Initialize(Vector3 origin, Transform target, float projectileSpeed)
    {
        this.origin = origin;
        this.target = target;
        this.projectileSpeed = projectileSpeed;

        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        float actualProjectileSpeed = 1 / projectileSpeed;

        //lerp to target position
        float currentTime = 0;
        while (currentTime < actualProjectileSpeed)
        {
            float positionAlongLerp = currentTime / actualProjectileSpeed;

            //update position
            transform.position = Vector3.Lerp(origin, target.position, positionAlongLerp);
            currentTime += Time.deltaTime;

            //wait for next frame
            yield return null;
        }

        //destroy the trail
        Destroy(gameObject);

        //wait for next frame
        yield return null;
    }
}

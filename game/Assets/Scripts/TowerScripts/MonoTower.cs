using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public abstract class MonoTower : MonoBehaviour
{

    [SerializeField] protected Transform firingPoint;
    [SerializeField] protected LayerMask enemyMask;
    protected TowerData towerData;

    public TowerData TowerData { set { towerData = value; } }


    /// <summary>
    /// finds the nearest valid transform
    /// </summary>
    /// <returns>Transform of nearest enemy</returns>
    protected GameObject FindNearestTarget()
    {
        //get all objects within range
        Collider[] hits = Physics.OverlapSphere(transform.position, towerData.attackRange, enemyMask);

        //prepare for check
        GameObject nearestObject = null;
        float nearestDistance = math.INFINITY;

        if (hits.Length <= 0) return null;
        foreach (Collider hit in hits)
        {
            GameObject hitObject = hit.gameObject;

            float distance = Vector3.Distance(transform.position, hitObject.transform.position);

            if (distance < nearestDistance)
            {
                nearestObject = hitObject;
                nearestDistance = distance;
            }
        }

        return nearestObject;
    }

    protected GameObject[] FindNearestNthTargets(int amount = 1)
    {
        //create array to return
        GameObject[] nearestObjects = new GameObject[amount];

        //get all objects within range
        List<Collider> hits = new(Physics.OverlapSphere(transform.position, towerData.attackRange, enemyMask));

        //returns null if no hits were made
        if (hits.Count <= 0) return null;

        //loop until the array is filled
        for (int i = 0; i < nearestObjects.Length; i++)
        {
            //prepare for check
            Collider nearestCollider = null;
            float nearestDistance = math.INFINITY;

            //go through all the colliders until you find the neares
            foreach (Collider hit in hits)
            {
                Collider hitCollider = hit;

                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);

                if (distance < nearestDistance)
                {
                    nearestCollider = hitCollider;
                    nearestDistance = distance;
                }
            }

            //save the nearest for return, and remove from hits list
            nearestObjects[i] = nearestCollider.gameObject;
            hits.Remove(nearestCollider);
        }


        return nearestObjects;
    }
    

    /// <summary>
    /// finds all the targets nearby
    /// </summary>
    /// <returns>an array of gameobjects that are within range</returns>
    protected GameObject[] FindAllTargets()
    {
        //get all objects within range
        Collider[] hits = Physics.OverlapSphere(transform.position, towerData.attackRange, enemyMask);
        GameObject[] targets = new GameObject[hits.Length];

        //converts all hits to their gameobjects
        for (int i = 0; i < hits.Length; i++)
        {
            targets[i] = hits[i].gameObject;
        }

        //return the new array
        return targets;
    }
}

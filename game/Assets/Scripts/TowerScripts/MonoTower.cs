using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;


public abstract class MonoTower : MonoBehaviour
{

    [SerializeField] protected Transform firingPoint;
    [SerializeField] protected LayerMask enemyMask;
    protected TowerData towerData;

    public TowerData TowerData { set { towerData = value; } }


    /// <returns>
    /// a sorted collection of the enemies by distance
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IEnumerable<GameObject> GetSortedList()
    {
        // get the tower position
        Vector2 towerPos = new(transform.position.x, transform.position.z);

        // get the sorted IEnumberable using a linq query
        IEnumerable<GameObject> objects =
            from obj in FindObjectsOfType<GameObject>() // get all the gameobjects in the scene
            where (obj.layer & enemyMask.value) != 0    // check whether the gameobject is within the enemy mask
            let enemyPos = new Vector2(obj.transform.position.x, obj.transform.position.y)  // get the 2D enemy position data
            let x = Math.Abs(towerPos.x - enemyPos.x)   // get the absolute relative x position
            let y = Math.Abs(towerPos.y - enemyPos.y)   // get the absolute relative y position
            orderby x * y   // order the list by the surface area of the rectangle made by the relative distance between the two; larger surface area = further away
            select obj;     // select the object

        return objects;
    }

    /// <summary>
    /// checks whether the enemy is within the specified range.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsInRange(GameObject obj)
    {
        Vector2 v1 = new(obj.transform.position.x, obj.transform.position.y);   // get the enemy's 2D position data
        Vector2 v2 = new(transform.position.x, transform.position.y);           // get the tower's 2D position data
        return Vector2.Distance(v1, v2) <= towerData.attackRange;               // calculate the distance and check whether the distance is within the range
    }

    /// <summary>
    /// find the nearest enemy and within the specified range
    /// </summary>
    /// <returns>Transform of nearest enemy, or null if none are in the range</returns>
    protected GameObject FindNearestTarget()
    {
        // get the first of the sorted list; aka, the closest
        GameObject enemy = GetSortedList().First();

        // return the enemy if it's within the range, otherwise return null
        if (IsInRange(enemy))
            return enemy;
        return null;
    }

    /// <summary>
    /// Find an amount of enemies which are nearest and within the specified range
    /// </summary>
    /// <returns>an array of the enemies that were closest</returns>
    protected GameObject[] FindNearestNthTargets(int amount = 1)
    {
        // get the amount of gameobjects in the sorted list
        IEnumerable<GameObject> enemyPool = GetSortedList().Take(amount);
        List<GameObject> enemySelected = new();

        foreach (GameObject enemy in enemyPool)
        {
            // if the enemy is not within the range, break; no more enemies will be useful
            if (IsInRange(enemy) == false)
                break;

            enemySelected.Add(enemy);
        }

        return enemySelected.ToArray();
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

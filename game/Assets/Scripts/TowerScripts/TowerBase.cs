using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;


public abstract class TowerBase : MonoBehaviour
{
    [SerializeField] protected Transform firingPoint;
    [SerializeField] protected LayerMask enemyMask;
    protected TowerData towerData;
    private List<GameObject> targets = null;
    private bool isAllowedToShoot = true;

    public TowerData TowerData { set { towerData = value; } }

    protected virtual void Start() => StartCoroutine(ShootLoop());
    protected virtual void FixedUpdate() => targets = SelectTargets(); //only try finding target every fixed updated (for fewer updates)

    protected abstract List<GameObject> SelectTargets();
    protected abstract void ShotTarget(GameObject target);

    private IEnumerator ShootLoop()
    {
        while (isAllowedToShoot)
        {
            //wait for shooting delay
            yield return new WaitForSeconds(1 / towerData.attackSpeed);

            foreach (GameObject target in targets)
                ShotTarget(target);

            yield return null;
        }
        yield return null;
    }


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
            where obj.activeInHierarchy                 // require object to be active
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
    /// Find an amount of enemies which are nearest and within the specified range
    /// </summary>
    /// <returns>an array of the enemies that were closest</returns>
    protected List<GameObject> FindNearestNthTargets(int amount = 1)
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

        return enemySelected;
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

#if UNITY_EDITOR
    // draw path for debuggong
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, towerData.attackRange);

        Gizmos.color = Color.red;
        if (targets != null)
        {
            foreach (GameObject target in targets)
            {
                Gizmos.DrawSphere(target.transform.position + new Vector3(0, 1, 0), 0.1f);
            }
        }
    }
#endif
}

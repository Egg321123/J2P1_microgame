using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class GameObjectUtils
{
    private static IEnumerable<GameObject> GetWithinRange(GameObject parent, float radius, IEnumerable<GameObject> pool)
    {
        foreach (GameObject obj in pool)
        {
            // if the object is not within the range, break; no more enemies will be useful
            if (IsWithinRadius(parent, obj, radius) == false)
                break;

            // return the object
            yield return obj;
        }
    }

    /// <summary>
    /// checks whether the game object's X/Z is within the radius.
    /// </summary>
    public static bool IsWithinRadius(GameObject parent, GameObject obj, float radius)
    {
        Vector2 v1 = new(obj.transform.position.x, obj.transform.position.y);   // get the enemy's 2D position data
        Vector2 v2 = new(parent.transform.position.x, parent.transform.position.y);           // get the tower's 2D position data
        return Vector2.Distance(v1, v2) <= radius;               // calculate the distance and check whether the distance is within the range
    }

    /// <returns>
    /// a sorted <see cref="GameObject"/> list from nearest to furthest relative to <paramref name="parent"/> on <paramref name="checkMask"/>. (X/Z plane)
    /// Ignores inactive objects
    /// </returns>
    public static IEnumerable<GameObject> GetSortedByDistance(GameObject parent, LayerMask layer)
    {
        // get the tower position
        Vector2 parentPos = new(parent.transform.position.x, parent.transform.position.z);

        // get the sorted IEnumberable using a linq query
        return
            from obj in GameObject.FindObjectsOfType<GameObject>() // get all the gameobjects in the scene
            where (obj.layer & layer.value) != 0    // check whether the gameobject is within the given mask
            where obj.activeInHierarchy             // require object to be active
            let pos = new Vector2(obj.transform.position.x, obj.transform.position.y)  // get the 2D found position data
            let x = Math.Abs(parentPos.x - pos.x)   // get the absolute relative x position
            let y = Math.Abs(parentPos.y - pos.y)   // get the absolute relative y position
            orderby x * y   // order the list by the surface area of the rectangle made by the relative distance between the two; larger surface area = further away
            select obj;     // select the object
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static List<GameObject> GetAllOnLayer(GameObject parent, LayerMask layer, float radius) => GetWithinRange(parent, radius, GetSortedByDistance(parent, layer)).ToList();
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static List<GameObject> GetNearestOnLayer(GameObject parent, LayerMask layer, float radius, int amount = 1) => GetWithinRange(parent, radius, GetSortedByDistance(parent, layer).Take(amount)).ToList();
}

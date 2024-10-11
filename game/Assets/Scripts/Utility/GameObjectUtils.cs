using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class GameObjectUtils
{
    /// <summary>
    /// checks whether the game object's X/Z is within the radius.
    /// </summary>
    public static bool IsWithinRadius(GameObject parent, GameObject obj, float radius)
    {
        Vector2 v1 = new(obj.transform.position.x, obj.transform.position.y);       // get the enemy's 2D position data
        Vector2 v2 = new(parent.transform.position.x, parent.transform.position.y); // get the tower's 2D position data
        return Vector2.Distance(v1, v2) <= radius;                                  // calculate the distance and check whether the distance is within the range
    }

    /// <returns>
    /// a sorted <see cref="GameObject"/> list from nearest to furthest relative to <paramref name="parent"/> on <paramref name="checkMask"/>. (X/Z plane)
    /// Ignores inactive objects
    /// </returns>
    public static IEnumerable<GameObject> GetObjects(GameObject parent, float radius, LayerMask layer)
    {
        // get the tower position
        Vector2 parentPos = new(parent.transform.position.x, parent.transform.position.z);

        // get the sorted IEnumberable using a linq query
        return
            from obj in GameObject.FindObjectsOfType<GameObject>()                      // get all the gameobjects in the scene
            where ((1 << obj.layer) & layer.value) != 0                                 // check whether the gameobject is within the given mask
            where obj.activeInHierarchy                                                 // require object to be active

            // check distance
            let pos = new Vector2(obj.transform.position.x, obj.transform.position.y)   // get the 2D found position data
            let dist = Vector2.Distance(parentPos, pos)                                 // calculate the distance
            where dist <= radius                                                        // check whether the distance is within the specified radius

            // order the list by distance
            orderby dist
            select obj;
    }

    public static IEnumerable<GameObject> GetObjectsCollider(GameObject parent, float radius, LayerMask layer)
    {
        // get the tower position
        Vector3 parentPos = parent.transform.position;

        Collider[] overlapTest = Physics.OverlapSphere(parentPos, radius, layer);
        List<GameObject> objects = overlapTest.Select(col => col.gameObject).ToList();

        return objects
        .Where(c => c.activeInHierarchy)                                           // Check if the object is active
        .OrderBy(c => Vector3.Distance(parentPos, c.transform.position));          // Sort by distance
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static List<GameObject> GetAllOnLayer(GameObject parent, float radius, LayerMask layer) => GetObjects(parent, radius, layer).ToList();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static List<GameObject> GetNearestOnLayer(GameObject parent, float radius, LayerMask layer, int amount = 1)
    {
        return GetObjects(parent, radius, layer).Take(amount).ToList();;
    }

    public static List<GameObject> GetNearestOnLayerCollider(GameObject parent, float radius, LayerMask layer, int amount = 1)
    {
        return GetObjectsCollider(parent, radius, layer).Take(amount).ToList();
    }
}

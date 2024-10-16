using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// creates a pool of objects which sets the inactive ones to active instead of continueously destroying and making new ones.
/// </summary>
public class ObjectPool<T> where T : MonoBehaviour
{
    // contains all the objects
    private List<T> pool = new();

    // obtains an object, if no inactive objects can be found, a new one is created.
    public T GetPooledObject(T prefab, Transform parent, Action<T> initializer = null)
    {
        for (int i = 0; i < pool.Count; i++)
            if (pool[i].isActiveAndEnabled == false) return pool[i];

        T obj = CreateNewObject(prefab, parent);
        initializer?.Invoke(obj); // call the initializer for extra initialization logic when the object is created.
        return obj;
    }

    // creates a new object
    private T CreateNewObject(T prefab, Transform parent)
    {
        T obj = GameObject.Instantiate(prefab, parent);

        pool.Add(obj);
        return obj;
    }
}

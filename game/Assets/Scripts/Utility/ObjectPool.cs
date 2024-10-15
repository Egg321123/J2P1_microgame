using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private List<T> pool = new();

    public T GetPooledObject(T prefab, Transform parent, Action<T> initializer = null)
    {
        for (int i = 0; i < pool.Count; i++)
            if (pool[i].isActiveAndEnabled == false) return pool[i];

        T obj = CreateNewObject(prefab, parent);
        initializer?.Invoke(obj);
        return obj;
    }

    private T CreateNewObject(T prefab, Transform parent)
    {
        T obj = GameObject.Instantiate(prefab, parent);

        pool.Add(obj);
        return obj;
    }
}

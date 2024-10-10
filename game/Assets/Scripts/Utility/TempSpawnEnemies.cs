using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSpawnEnemies : MonoBehaviour
{
    [SerializeField] private GameObject Enemy;
    [SerializeField] private bool spawnEnemies = true;
    [SerializeField] List<GameObject> pooledEnemies = new();

    // Start is called before the first frame update
    void Start()
    {
        //makes sure that there is at least one instance available
        CreateNewPooledObject();

        //starts the spawen cycle
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (spawnEnemies)
        {
            float randomWaitTime = Random.Range(0.05f, .2f);
            yield return new WaitForSeconds(randomWaitTime);

            GameObject enemy = GetPooledObject();
            enemy.SetActive(true);
            enemy.GetComponent<FollowPath>().StartWalking();

            yield return null;
        }

        yield return null;
    }

    /// <summary>
    /// gets an object that's available for pooling
    /// </summary>
    /// <returns></returns>
    private GameObject GetPooledObject()
    {
        //search for available objects
        for (int i = 0; i < pooledEnemies.Count; i++)
        {
            if (!pooledEnemies[i].activeInHierarchy) return pooledEnemies[i];
        }

        //if no object is available, create new one
        return CreateNewPooledObject();
    }


    /// <summary>
    /// instantiate an object and set it up for pooling
    /// </summary>
    /// <returns></returns>
    private GameObject CreateNewPooledObject()
    {
        GameObject obj = Instantiate(Enemy);
        obj.SetActive(false);
        pooledEnemies.Add(obj);
        obj.transform.parent = transform;

        return obj;
    }
}

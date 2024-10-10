using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// if the wave is over, the data is saved.
// if the last wave has been reached, a new level is generated
public class Waves : MonoBehaviour
{
    [SerializeField, Min(1)] private int waves = 10;
    [SerializeField, Min(1)] private int spawnEnemies = 50;
    [SerializeField, Min(0)] private float enemySpawnRate = 1F;

    [SerializeField] private GameObject[] enemies;
    private List<GameObject>[] enemyPools;


    // called when the script is being loaded
    private void Awake()
    {
        enemyPools = new List<GameObject>[enemies.Length];

        // populate the array with lists
        for (int i = 0; i < enemies.Length; i++)
            enemyPools[i] = new List<GameObject>();
    }

    // called before the first frame
    private void Start()
    {
        NextWave();
    }

    public void NextWave()
    {
        StartCoroutine(SpawnLoop());
    }

    // spawns the enemies
    private IEnumerator SpawnLoop()
    {
        for (int i = 0; i < spawnEnemies; i++)
        {
            // await the spawn rate
            yield return new WaitForSeconds(enemySpawnRate);

            // get a random enemy
            int index = Random.Range(0, enemies.Length);
            GameObject enemy = GetPooledEnemy(index);
            enemy.SetActive(true);
            enemy.GetComponent<FollowPath>().StartWalking();

            yield return null;
        }

        yield return null;
    }

    // gets an enemy inside of the pool
    private GameObject GetPooledEnemy(int index)
    {
        //search for available objects
        for (int i = 0; i < enemyPools[index].Count; i++)
        {
            if (!enemyPools[index][i].activeInHierarchy) return enemyPools[index][i];
        }

        return CreateNewEnemy(index);
    }

    // creates a new enemy instance and adds it to the pool
    private GameObject CreateNewEnemy(int index)
    {
        // create a new instance of the enemy set to false
        GameObject obj = Instantiate(enemies[index], transform);
        obj.SetActive(false);

        // add it to the pool and return the object
        enemyPools[index].Add(obj);
        return obj;
    }
}

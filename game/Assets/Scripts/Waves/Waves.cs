using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<GameObject> allEnemies = new();


    // called when the script is being loaded
    private void Awake()
    {
        if (GameManager.Instance.Waves != null) throw new InvalidOperationException("an instance of waves already exists");
        GameManager.Instance.Waves = this;

        // create the enemy pools
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

    public IEnumerable<GameObject> GetEnemiesInRadius(Vector3 pos, float radius, int count = -1)
    {
        if (count <= 0)
            count = allEnemies.Count;

        return (
            from enemy in allEnemies.AsEnumerable()
            where enemy.activeInHierarchy

            let dist = Vector3.Distance(pos, enemy.transform.position)
            where dist <= radius

            orderby dist
            select enemy
        ).Take(count);
    }
    // draw path for debuggong
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetEnemiesInRadius(Vector3.zero, float.PositiveInfinity, 1).First().transform.position + new Vector3(0, 1, 0), 0.1f);
    }

    // spawns the enemies
    private IEnumerator SpawnLoop()
    {
        for (int i = 0; i < spawnEnemies; i++)
        {
            // await the spawn rate
            yield return new WaitForSeconds(enemySpawnRate);

            // get a random enemy
            int index = UnityEngine.Random.Range(0, enemies.Length);
            GameObject enemy = GetPooledEnemy(index);
            enemy.GetComponent<EnemyBase>().Initialize(1);

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
        allEnemies.Add(obj);
        return obj;
    }
}

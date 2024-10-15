using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random; // define which random we wanna use

// if the wave is over, the data is saved.
// if the last wave has been reached, a new level is generated
public class Waves : MonoBehaviour
{
    private const int MAX_SPAWNS = 1000;

    [SerializeField] private GameObject winUI = null;           // the UI that is shown when the game is won
    [SerializeField, Min(0)] private float enemySpawnRate = 1F; // how many times per second to spawn a new enemy
    [SerializeField] private EnemyTypeData[] enemyTypes = null; // the different enemy types
    [SerializeField] private SpawnData[] waves = null;          // the different waves constructed

    private ObjectPool<EnemyBase>[] enemyPools = null;          // the pools for enemy pooling
    private List<EnemyBase> allEnemies = new();                 // contains all the enemies

    // refrences
    private MonoLevel monoLevel = null;                         // reference to the MonoLevel for regenerating the level on win

    // property shorthands
    private Save Save => GameManager.Instance.Save;
    private int Wave { get => Save.data.wave; set => Save.data.wave = value; }
    private int Level { get => Save.data.level; set => Save.data.level = value; }
    private bool newLevel = false;


    public void NextWave()
    {
        if (newLevel == true)
        {
            monoLevel.RegenerateLevel(Level, Save.data.towers);     // regenerate the level
            newLevel = false;
        }

        winUI.SetActive(false);                                     // hide the win UI (again)

        Debug.Log($"started wave {Wave}");
        StartCoroutine(SpawnEnemies(Wave));
    }

    // get the enemies within a radius
    public IEnumerable<EnemyBase> GetEnemiesInRadius(Vector3 pos, float radius, int count = -1)
    {
        if (count < 0)
            count = allEnemies.Count;

        return (
            from enemy in allEnemies.AsEnumerable() // loop through the enemies
            where enemy.OpenForPooling == false     // check whether the enemy is alive

            // ensure that the enemy resides within a certain radius
            let dist = Vector3.Distance(pos, enemy.transform.position)
            where dist <= radius

            orderby dist
            select enemy
        ).Take(count);
    }

    // spawns the enemies
    private IEnumerator SpawnEnemies(int wave)
    {
        List<EnemySpawnData> spawnData = waves[wave].enemySpawnData.ToList(); // create a copy of the enemy spawn data as a list for this wave to operate on

        int spawnCount = 0; // for the spawn cap, in the case that the loop gets stuck
        while (spawnData.Count > 0)
        {
            // await the spawn rate
            yield return new WaitForSeconds(1.0F / enemySpawnRate);

            // enemy spawning
            {
                // get a random spawn from the spawn data
                int index = Random.Range(0, spawnData.Count);
                EnemySpawnData spawn = spawnData[index];

                // spawn the enemy and decrease the count
                SpawnEnemy(spawn.difficulty);
                spawn.count--;

                spawnData[index] = spawn;

                // if the count is <= 0, remove the spawn data of this difficulty alltogether.
                if (spawn.count <= 0)
                    spawnData.RemoveAt(index);
            }

            // spawn cap protection
            if (spawnCount >= MAX_SPAWNS)
                throw new IndexOutOfRangeException($"spawncap of '{MAX_SPAWNS}' has been reached! Could not continue.");
            spawnCount++;
            yield return null;
        }

        // wave spawning is done; await when all enemies have been killed
        StartCoroutine(WinCheck());

        yield return null;
    }

    private IEnumerator WinCheck()
    {
        // check whether the enemy is open for pooling
        while (allEnemies.Where(e => e.IsAlive).Count() > 0)
            yield return new WaitForFixedUpdate();

        Wave++;

        // increase the level if the waves have been reached
        if (Wave >= waves.Length)
        {
            Wave = 0;                       // reset wave
            Level++;                        // increase the level
            monoLevel.Level.ClearLevel();   // clear the level so we don't save towers

            newLevel = true;
            Debug.Log("progressed level");
        }

        winUI.SetActive(true);                  // set the Win UI active
        GameManager.Instance.Save.SaveFile();   // save the current state to the file

        yield return null;
    }

    // spawns an enemy
    private void SpawnEnemy(EnemyDifficulty difficulty)
    {
        // create a list for the indices of the enemies with the capacity set to the amount of existing enemy types
        // because that is the max it will ever be so we don't have to redefine the internal array when adding items.
        List<int> enemyIndices = new(enemyTypes.Length);

        // get the indices of the enemies that match the difficult requested
        for (int i = 0; i < enemyTypes.Length; i++)
            if (enemyTypes[i].difficulty == difficulty)
                enemyIndices.Add(i);

        // get a random index
        int index = Random.Range(0, enemyIndices.Count);

        // use the index to get an enemy within the pool
        ObjectPool<EnemyBase> pool = enemyPools[index];
        EnemyBase enemy = pool.GetPooledObject(enemyTypes[index].enemy, transform, enemy => allEnemies.Add(enemy));

        // initialize the enemy
        enemy.Initialize(GameManager.Instance.Save.data.level);
    }

    // called when the script is being loaded
    private void Awake()
    {
        // add self to the game manager
        if (GameManager.Instance.Waves != null) throw new InvalidOperationException("an instance of waves already exists");
        GameManager.Instance.Waves = this;

        // get monobehaviours
        monoLevel = FindFirstObjectByType<MonoLevel>();

        // create the enemy pools
        enemyPools = new ObjectPool<EnemyBase>[enemyTypes.Length];

        // populate the enemy pool array with the object pools
        for (int i = 0; i < enemyTypes.Length; i++)
            enemyPools[i] = new ObjectPool<EnemyBase>();
    }
}

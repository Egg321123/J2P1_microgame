using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random; // define which random we wanna use

// if the wave is over, the data is saved.
// if the last wave has been reached, a new level is generated
public class Waves : MonoBehaviour
{
    [Header("UI stuff")]
    [SerializeField] private GameObject winUI = null;           // the UI that is shown when the game is won
    [SerializeField] private GameObject LoseUI = null;          // the UI that is shown when u didn't survive the level
    [SerializeField] private TextMeshProUGUI counter = null;    // for showing a count down between waves
    [SerializeField, Min(0)] private int waveDelaySeconds = 5;  // the delay in seconds between waves


    [Header("enemy spawning")]
    [SerializeField, Min(0)] private float enemySpawnRate = 1F; // how many times per second to spawn a new enemy
    [SerializeField] private EnemyTypeData[] enemyTypes = null; // the different enemy types
    [SerializeField] private SpawnData[] waves = null;          // the different waves constructed

    // event
    public event Action NewWave;
    public event Action HealthDecreased;

    // enemy storage
    private ObjectPool<EnemyBase>[] enemyPools = null;          // the pools for enemy pooling
    private List<EnemyBase> allEnemies = new();                 // contains all the enemies

    // refrences
    private MonoLevel monoLevel = null;                         // reference to the MonoLevel for regenerating the level on win
    private Shop shop = null;                                   // reference to the shop so we can disable it

    // property shorthands
    public int MaxWaves => waves.Length;
    private Save Save => GameManager.Instance.Save;
    public int Wave { get => Save.data.wave; set => Save.data.wave = value; }
    private int Level { get => Save.data.level; set => Save.data.level = value; }
    private bool newLevel = false;


    public void NextWave()
    {
        if (Wave == 0) GameManager.Instance.Save.data.moneyStartLVL = GameManager.Instance.Save.data.money;

        if (newLevel == true)
        {
            monoLevel.RegenerateLevel(Level, Save.data.towers);     // regenerate the level
            newLevel = false;
        }

        winUI.SetActive(false);                                     // hide the win UI (again)
        shop.ShopToggle(true);                                      // make the shop active again

        StartCoroutine(SpawnEnemies(Wave));
        NewWave?.Invoke();
        Debug.Log($"started wave {Wave + 1}/{waves.Length} in level {Level + 1}");
    }
    public void TryAgain()/*Dani�l*/
    {
        //acitvate and deactivate the UI so the player can paly again
        Time.timeScale = 1.0f;
        LoseUI.SetActive(false);
        
        NextWave();
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

            yield return null;
        }

        // wave spawning is done; await when all enemies have been killed
        StartCoroutine(WinCheck());

        yield return null;
    }

    // checks whether the win condition has been met and all the enemies are no longer alive. (is called after the last enemy has been spawned :3)
    private IEnumerator WinCheck()
    {
        // hold the thread for as it takes till the amount of enemies alive is 0
        while (allEnemies.Where(e => e.IsAlive).Count() > 0)
            yield return new WaitForFixedUpdate();  // check every fixed update, as we don't need to check every frame. :3

        // all enemies have perished, increase the wave!
        Wave++;

        // increase the level if the waves have been reached
        if (Wave >= waves.Length)
        {
            Wave = 0;                       // reset wave
            Level++;                        // increase the level
            monoLevel.Level.ClearLevel();   // clear the level so we don't save towers

            newLevel = true;
            Debug.Log($"progressed to level {Level}!");
            winUI.SetActive(true);                  // set the Win UI active
            shop.ShopToggle(false);                 // disable the shop

        }

        GameManager.Instance.Save.SaveFile();   // save the current state to the file

        if (newLevel == false)
        {
            counter.gameObject.SetActive(true);
            shop.ShopToggle(false);

            for (int i = 0; i < waveDelaySeconds; i++)
            {
                counter.text = (waveDelaySeconds - i).ToString();
                yield return new WaitForSecondsRealtime(1.0F);
            }

            counter.gameObject.SetActive(false);
            NextWave(); // shop is set active in this method
        }

        yield return null;
    }
    public void LoseCheck()/*Dani�l*/
    {
        HealthDecreased?.Invoke();
        if (Save.data.hp > 0) return;

        StopAllCoroutines();
        foreach (EnemyBase enemy in allEnemies) if (enemy.IsAlive) enemy.DisableEnemy();
        Wave = 0;

        //switch between UI
        LoseUI.SetActive(true);
        shop.ShopToggle(false);
        //clear all the saved data or this level, clear the 

        Save.ResetLevelData();
        GameManager.Instance.Save.data.money = GameManager.Instance.Save.data.moneyStartLVL;
        GameManager.Instance.Save.SaveFile();
        monoLevel.RegenerateLevel(Level, Save.data.towers);

        Time.timeScale = 0;
    }

    // spawns an enemy of a specific difficulty
    private void SpawnEnemy(EnemyDifficulty difficulty)
    {
        // create a list for the indices of the enemies with the capacity set to the amount of existing enemy types
        // because that is the max it will ever be so we don't have to redefine the internal array when adding items.
        List<int> enemyIndices = new(enemyTypes.Length);

        // get the indices of the enemies that match the difficult requested
        for (int i = 0; i < enemyTypes.Length; i++)
            if (enemyTypes[i].difficulty == difficulty)
                enemyIndices.Add(i);

        // get a random index of the previoursly selected indecencies
        int index = Random.Range(0, enemyIndices.Count);

        // use the index to get an enemy within the pool
        int targetIndex = enemyIndices[index];
        ObjectPool<EnemyBase> pool = enemyPools[targetIndex];
        EnemyBase enemy = pool.GetPooledObject(enemyTypes[targetIndex].enemy, transform, enemy => allEnemies.Add(enemy));

        // initialize the enemy
        enemy.Initialize(GameManager.Instance.Save.data.level);
    }

    // called when the script is being loaded
    private void Awake()
    {
        // add self to the game manager!
        if (GameManager.Instance.Waves != null) throw new InvalidOperationException("an instance of waves already exists");
        GameManager.Instance.Waves = this;

        // get monobehaviours
        monoLevel = FindFirstObjectByType<MonoLevel>();
        shop = FindFirstObjectByType<Shop>();

        // create the enemy pools for each different type
        enemyPools = new ObjectPool<EnemyBase>[enemyTypes.Length];

        // populate the enemy pool array with the object pools
        for (int i = 0; i < enemyTypes.Length; i++)
            enemyPools[i] = new ObjectPool<EnemyBase>();
    }
}

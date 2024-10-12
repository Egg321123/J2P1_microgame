using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // there is no isAlive bool as it's more performance friendly to just check if gameObject.isActiveInHierarchy,
    // as it doesn't require memory to be allocated

    [SerializeField] EnemySpawnData spawnData;
    EnemyMovementSystem movement;

    // enemy stats
    public int Level { get; private set; }
    public int Health { get; private set; }
    public float Speed { get; private set; }
    public Vector3 RandomOffset { get; private set; }

    // other utility
    private int targetNodeIndex = 0;

    private void Awake()
    {
        // create a random offset to make it neater
        float randomX = Random.Range(-0.25f, 0.25f);
        float randomZ = Random.Range(-0.25f, 0.25f);
        RandomOffset = new(randomX, 0, randomZ);

        movement = FindFirstObjectByType<EnemyMovementSystem>();
    }

    /// <summary>
    /// run either when instancing this object, or when grabbing it from the pool
    /// </summary>
    /// <param name="level"></param>
    /// <param name="data"></param>
    public void Initialize(int level)
    {
        // sets the values back to needed values
        Level = level;
        Health = spawnData.GetHealth(level);
        Speed = spawnData.GetSpeed(level);

        // prepare object and add to movement job system
        targetNodeIndex = 0;
        gameObject.SetActive(true);
        StartMoving();
    }

    // health stuff

    /// <summary>
    /// allows the enemy to take damage, also handles the enemy dying automatically
    /// </summary>
    /// <param name="amount"></param>
    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0) Kill();
    }

    /// <summary>
    /// "kills" the enemy, allows for the enemy to be used in pooling again NEVER destroy the enemy
    /// </summary>
    private void Kill()
    {
        // hides the enemy and remove it from movement job system
        gameObject.SetActive(false);
        movement.RemoveEnemy(this);
    }

    //allows the movement script interact back to the enemy to tell it that it reached the end, for now just kills
    public void HasReachedEnd()
    {
        Kill();
        //damage player or smthn
    }
    public int TargetNodeIndex { get { return targetNodeIndex; } set { targetNodeIndex = value; } }

    //stuff that allows you to interact with the movement script
    protected void StartMoving() => movement.AddEnemy(this);
    protected void StopMoving() => movement.RemoveEnemy(this);
    
}

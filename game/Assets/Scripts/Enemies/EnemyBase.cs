using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] EnemySpawnData spawnData;
    EnemyMovementSystem movement;


    //enemy stats
    public int Level { get; private set; }
    public int Health { get; private set; }
    public float Speed { get; private set; }
    public Vector3 randomOffset { get; private set; }

    private int targetNodeIndex = 0;
    public int TargetNodeIndex { get { return targetNodeIndex; } set { targetNodeIndex = value; } }

    private void Awake()
    {
        //create a random offset to make it neater
        float randomX = Random.Range(-0.25f, 0.25f);
        float randomZ = Random.Range(-0.25f, 0.25f);
        randomOffset = new(randomX, 0, randomZ);

        movement = FindFirstObjectByType<EnemyMovementSystem>();
    }

    /// <summary>
    /// run either when instancing this object, or when grabbing it from the pool
    /// </summary>
    /// <param name="level"></param>
    /// <param name="data"></param>
    public void Initialize(int level)
    {
        //sets the values back to needed values
        this.Level = level;
        Health = spawnData.GetHealth(level);
        Speed = spawnData.GetSpeed(level);

        Revive();
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0) Kill();
    }

    public void Kill()
    {
        gameObject.SetActive(false);
        transform.rotation = Quaternion.identity;
        targetNodeIndex = 0;

        //remove from movement job system
        movement.RemoveEnemy(this);
    }

    public void Revive()
    {
        gameObject.SetActive(true);
        transform.rotation = Quaternion.identity;
        targetNodeIndex = 0;

        //add to movement job system
        movement.AddEnemy(this);
    }

    public void HasReachedEnd()
    {
        //damage player or smthn
        print("has reached end");
        Kill();
    }
}

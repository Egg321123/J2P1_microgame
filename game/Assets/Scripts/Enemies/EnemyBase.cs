using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] private string enemyName;

    [Header("default values")]
    [SerializeField] private int baseHealth = 10;
    [SerializeField] private float baseSpeed = 1;

    [Header("Level scaling stats")]
    [SerializeField] private int extraHealthPerLevel = 1;
    [SerializeField] private float extraSpeedPerLevel = 0.1f;

    //saves the movement script that controls all ai
    EnemyMovementSystem movement;

    // enemy stats
    public int Health { get; private set; }
    public float Speed { get; private set; }
    public Vector3 RandomOffset { get; private set; }

    // utility
    public bool OpenForPooling { get; private set; } // OpenForPooling = false is the same as this enemy being alive
    [HideInInspector] public int TargetNodeIndex = 0;



    private void Awake()
    {
        // create a random offset to make it look like the enemies are not strictly following the path
        float randomX = Random.Range(-0.25f, 0.25f);
        float randomZ = Random.Range(-0.25f, 0.25f);
        RandomOffset = new(randomX, 0, randomZ);

        //get a reference to the movement script
        movement = FindFirstObjectByType<EnemyMovementSystem>();

        //once everything is done, ready for pooling
        OpenForPooling = true;

        //hide the object
        gameObject.SetActive(false);
    }

    /// <summary>
    /// run either when instancing this object, or when grabbing it from the pool
    /// </summary>
    /// <param name="level"></param>
    /// <param name="data"></param>
    public virtual void Initialize(int level)
    {
        //mark this as an unavailable object to spawn, because it's already being spawned
        OpenForPooling = false;

        // sets the values back to needed values
        Health = baseHealth + (extraHealthPerLevel * level);
        Speed = baseSpeed + (extraSpeedPerLevel * level);

        //prepare for moving again, make sure to reset these values, otherwise the movement script might get angry
        TargetNodeIndex = 0;
        transform.position = movement.PathStart;
        gameObject.SetActive(true);
        StartMoving();
    }



    /// <summary>
    /// allows the enemy to take damage, also handles the enemy dying automatically
    /// </summary>
    /// <param name="amount"></param>
    public virtual void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0) Kill();
    }
    /// <summary>
    /// "kills" the enemy, base method prepares it for pooling (respawning)
    /// </summary>
    protected virtual void Kill() => StartCoroutine(PrepareForPooling());



    /// <summary>
    /// used by the movement script only, triggers when the enemy reaches the end of the path, so where the player is.
    /// </summary>
    public void HasReachedEnd()
    {
        Kill();
        //damage player or smthn
    }



    /// <summary>
    /// allows this enemy to move.
    /// </summary>
    protected void StartMoving() => movement.AddEnemy(this);
    /// <summary>
    /// stops this enemy from moving
    /// </summary>
    protected void StopMoving() => movement.RemoveEnemy(this);


    /// <summary>
    /// handles the pooling behavior of this enemy, removes it from the movement behavior, and mark is as OpenForPooling
    /// </summary>
    /// <returns></returns>
    protected IEnumerator PrepareForPooling()
    {
        StopMoving();
        yield return new WaitForEndOfFrame();

        // tell other scripts that it's ready for pooling again, and hide this object
        OpenForPooling = true;
        gameObject.SetActive(false);

        yield return null;
    }
}

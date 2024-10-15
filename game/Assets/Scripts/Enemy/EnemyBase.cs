using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] private string enemyName;

    [Header("default values")]
    [SerializeField] private int baseHealth = 10;
    [SerializeField] private float baseSpeed = 1;
    [SerializeField] private int baseDroppedMoney = 10;

    [Header("Level scaling stats")]
    [SerializeField] private int extraHealthPerLevel = 1;
    [SerializeField] private float extraSpeedPerLevel = 0.1f;
    [SerializeField] private int extraDroppedMoneyPerLevel = 1;

    //saves the movement script that controls all ai
    EnemyMovementSystem movement;
    MoneyHandler moneyHandler;

    // enemy stats
    private int health;
    private int droppedMoney;
    public float Speed { get; private set; }
    public Vector3 RandomOffset { get; private set; }

    // utility
    public bool OpenForPooling { get; private set; } // OpenForPooling = false is the same as this enemy being alive
    public bool IsAlive => !OpenForPooling;
    [HideInInspector] public int TargetNodeIndex = 0;
    private bool isStunned = false;



    private void Awake()
    {
        // create a random offset to make it look like the enemies are not strictly following the path
        float randomX = Random.Range(-0.25f, 0.25f);
        float randomZ = Random.Range(-0.25f, 0.25f);
        RandomOffset = new(randomX, 0, randomZ);

        //get a reference to the movement script
        movement = FindFirstObjectByType<EnemyMovementSystem>();
        moneyHandler = FindFirstObjectByType<MoneyHandler>();

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
        health = baseHealth + (extraHealthPerLevel * level);
        Speed = baseSpeed + (extraSpeedPerLevel * level);
        droppedMoney = baseDroppedMoney + (extraDroppedMoneyPerLevel * level);

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
        health -= amount;
        if (health <= 0) Kill();
    }
    /// <summary>
    /// "kills" the enemy, base method prepares it for pooling (respawning)
    /// </summary>
    protected virtual void Kill()
    {
        GameManager.Instance.Save.data.stats.IncreaseKills();
        moneyHandler.Earn(droppedMoney);
        if (!OpenForPooling) StartCoroutine(PrepareForPooling());
    }

    public void ApplyStun(float length)
    {
        if (!isStunned) StartCoroutine(StunMoveDelay(length));
    }

    private IEnumerator StunMoveDelay(float length)
    {
        isStunned = true;
        StopMoving();

        yield return new WaitForSeconds(length);

        if (!OpenForPooling) StartMoving();
        isStunned = false;

        yield return null;
    }


    /// <summary>
    /// used by the movement script only, triggers when the enemy reaches the end of the path, so where the player is.
    /// </summary>
    public void HasReachedEnd()
    {
        if (!OpenForPooling) StartCoroutine(PrepareForPooling());
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

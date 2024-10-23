using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] private GameObject audioPrefab;
    [SerializeField] private AudioClip clip;

    [Header("default values")]
    [SerializeField] private int baseHealth = 10;
    [SerializeField] private float baseSpeed = 1;
    [SerializeField] private int baseDroppedMoney = 10;

    [Header("Level scaling stats")]
    [SerializeField] private int extraHealthPerLevel = 1;
    [SerializeField] private float extraSpeedPerLevel = 0.1f;
    [SerializeField] private int extraDroppedMoneyPerLevel = 1;

    //saves the movement script that controls all ai
    private EnemyMovementSystem movement;           // for moving the enemy
    private MoneyHandler moneyHandler;              // for adding money when the enemy dies
    public GameObject deathParticles;               // the particles that spawn in when the enemy dies

    // enemy stats
    private int health;
    private int droppedMoney;
    protected Vector3 randomOffset = Vector3.zero;
    public float Speed { get; private set; }
    public Vector3 RandomOffset { get { return randomOffset; } }

    // utility
    public bool OpenForPooling { get; private set; }    // whether the enemy is allowed to be pooled OpenForPooling = false is the same as this enemy being alive
    public bool IsAlive => !OpenForPooling;             // says whether the enemy is alive or not
    [HideInInspector] public int TargetNodeIndex = 0;   // the current target node
    private bool isStunned = false;                     // whether the enemy has been stunned

    protected virtual void Update() { }

    // awake is called when the script is being loaded
    protected virtual void Awake()
    {
        // create a random offset to make it look like the enemies are not strictly following the path
        float randomX = Random.Range(-0.25f, 0.25f);
        float randomZ = Random.Range(-0.25f, 0.25f);
        randomOffset = new(randomX, 0, randomZ);

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
        GameObject sound = Instantiate(audioPrefab, transform.position, Quaternion.identity);
        sound.GetComponent<AudioClipPlayer>().Initialize(clip, 200);

        GameManager.Instance.Save.data.stats.IncreaseKills();
        moneyHandler.Earn(droppedMoney);

        DisableEnemy();
        Instantiate(deathParticles, transform.position, Quaternion.identity);
    }

    public void DisableEnemy()
    {
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
        print("has reach end triggered");

        DisableEnemy();

        // damage the player
        GameManager.Instance.Save.data.hp--;
        GameManager.Instance.Waves.LoseCheck();
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

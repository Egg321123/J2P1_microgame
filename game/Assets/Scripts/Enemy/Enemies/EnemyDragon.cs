using UnityEngine;

public class EnemyDragon : EnemyBase
{
    [SerializeField] private GameObject animTarget;
    private float enemyTime = 0;
    private Vector3 verticalOffset = Vector3.zero;
    private float randomVerticalOffset = 1;

    protected override void Awake()
    {
        randomVerticalOffset += Random.Range(-0.5f, 0.5f);
        enemyTime = Random.Range(0f, 100f);
        base.Awake();
    }

    protected override void Update()
    {
        enemyTime += Time.deltaTime;

        verticalOffset.y = (Mathf.Sin(enemyTime * 1) * 0.3f) + randomVerticalOffset;
        animTarget.transform.transform.localPosition = verticalOffset;

        base.Update();
    }

    //your own initialize behavior, for example if you want it to do fancy particles when it spawns
    public override void Initialize(int level)
    {
        //runs the code that initialize usually does
        base.Initialize(level);
    }

    //your own take damage behavior
    public override void TakeDamage(int amount)
    {
        //does what the take damage does in the base method
        base.TakeDamage(amount);
    }

    //your own kill behavior
    protected override void Kill()
    {
        //does what the kill does in the base method
        base.Kill();
    }

}

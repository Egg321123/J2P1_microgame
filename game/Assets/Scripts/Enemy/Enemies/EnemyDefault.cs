public class EnemyDefault : EnemyBase
{
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

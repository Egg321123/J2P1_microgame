/// <summary>
/// defines what "difficulty" an enemy is, mainly used for catagorising the different enemies for spawning
/// </summary>
public enum EnemyDifficulty
{
    EASY,
    MEDIUM,
    HARD,

    // utility definitions
    MAX = HARD,
    MIN = EASY,
}

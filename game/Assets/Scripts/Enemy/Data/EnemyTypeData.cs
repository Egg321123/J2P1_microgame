using System;

/// <summary>
/// aids with catagorising the enemies by the difficulty
/// </summary>
[Serializable]
public struct EnemyTypeData
{
    public EnemyDifficulty difficulty;
    public EnemyBase enemy;
}

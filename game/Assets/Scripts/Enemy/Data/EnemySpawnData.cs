using System;

/// <summary>
/// contains what difficulty enemy to spawn with a certain count
/// </summary>
[Serializable]
public struct EnemySpawnData
{
    public EnemyDifficulty difficulty;
    public int count;
}

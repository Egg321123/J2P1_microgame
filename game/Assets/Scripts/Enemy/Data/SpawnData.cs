using System;

// basically unity serialization being annoying, what can I say?
[Serializable]
public struct SpawnData
{
    public EnemySpawnData[] enemySpawnData;
}

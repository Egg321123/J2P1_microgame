using System;
using UnityEngine;

// basically unity serialization being annoying, what can I say?
[Serializable]
public struct SpawnData
{
    [Header("what enemies to spawn during this wave")]
    public EnemySpawnData[] enemySpawnData;
}

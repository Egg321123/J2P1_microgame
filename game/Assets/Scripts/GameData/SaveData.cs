using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SaveData
{
    // add more save data here
    public Statistics stats;
    public int level;
    public int money;
    public List<Vector2Int> towers;

    public void Initialize()
    {
        // assign the default values
        level = 0;
        money = 0;

        // initialize statistics with their default values
        stats = new Statistics();
        stats.Initialize();
    }
}

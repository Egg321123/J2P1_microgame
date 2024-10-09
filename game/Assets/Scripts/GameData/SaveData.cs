using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SaveData
{
    // add more save data here
    public Statistics stats;
    public int level;
    public int wave;
    public int money;
    public TileData[] towers;

    public void Initialize()
    {
        // assign the default values
        level = 0;
        wave = 0;
        money = 0;
        towers = null;

        // initialize statistics with their default values
        stats = new Statistics();
        stats.Initialize();
    }
}

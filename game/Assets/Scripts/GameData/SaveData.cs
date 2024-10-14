using System;

[Serializable]
public struct SaveData
{
    // add more save data here
    public Statistics stats;
    public int level;
    public int wave;
    public long money;
    public TileData[] towers;

    public void Initialize()
    {
        // assign the default values
        level = 0;
        wave = 0;
        money = 250000;
        towers = new TileData[0];

        // initialize statistics with their default values
        stats = new Statistics();
        stats.Initialize();
    }
}

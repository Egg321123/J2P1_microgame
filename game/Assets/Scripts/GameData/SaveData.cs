using System;

[Serializable]
public struct SaveData
{
    // add more save data here
    public Statistics stats;
    public int hp;
    public int level;
    public int wave;
    public long money;
    public TileData[] towers;


    public void Initialize()
    {
        // assign the default values
        hp = 10;
        level = 0;
        wave = 0;
        money = 100;
        towers = new TileData[0]; // initialize the towers as an empty array, so it serializes properly with 0 towers, and also doesn't cause other nasty errors

        // initialize statistics with their default values
        stats = new Statistics();
        stats.Initialize();
    }
}

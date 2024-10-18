using System;

[Serializable]
public struct SaveData
{
    public const int MAX_HP = 10;

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
        hp = MAX_HP;
        level = 0;
        wave = 0;
        money = 500;
        towers = new TileData[0]; // initialize the towers as an empty array, so it serializes properly with 0 towers, and also doesn't cause other nasty errors

        // initialize statistics with their default values
        stats = new Statistics();
        stats.Initialize();
    }
}

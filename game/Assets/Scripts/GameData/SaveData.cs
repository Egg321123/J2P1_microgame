using System;

[Serializable]
public struct SaveData
{
    public const int MAX_HP = 10;
    public const int DATA_VERSION = 0;      // change this every time you make a change to the stored data

    // add more save data here
    public int dataVersion;
    public int hp;
    public int level;
    public int wave;
    public long money;
    public long moneyStartLVL;//
    public int[] towerBoughtCount;
    public TileData[] towers;
    public Statistics stats;


    public void Initialize()
    {
        // utility, used to keep track of what version the file is
        dataVersion = DATA_VERSION;

        // assign the default values
        hp = MAX_HP;
        level = 0;
        wave = 0;
        money = 500;
        towers = new TileData[0]; // initialize the towers as an empty array, so it serializes properly with 0 towers, and also doesn't cause other nasty errors
        towerBoughtCount = new int[(int)TowerType.TOWER_COUNT - 1]; // initializes the array with zeroes

        // initialize statistics with their default values
        stats = new Statistics();
        stats.Initialize();
    }
}

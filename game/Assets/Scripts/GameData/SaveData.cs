using System;

[Serializable]
public struct SaveData
{
    // add more save data here
    public Statistics stats;

    public void Initialize()
    {
        // assign the default values

        // initialize statistics with their default values
        stats = new Statistics();
        stats.Initialize();
    }
}

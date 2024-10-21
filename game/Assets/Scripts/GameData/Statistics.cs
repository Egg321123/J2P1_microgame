using System;

[Serializable]
public struct Statistics
{
    //current level
    public int kills;
    public int towersPlaced;
    public int towersUpgraded;
    public long moneySpent;
    public int deaths;

    //total
    public int totalDeaths;
    public int totalKills;
    public int totalTowersPlaced;
    public int totalTowersUpgraded;
    public long totalMoneySpent;

    public void Initialize()
    {
        // assign the default values
        deaths = 0;
        kills = 0;
        towersPlaced = 0;
        towersUpgraded = 0;
        moneySpent = 0;

        totalDeaths = 0;
        totalKills = 0;
        totalTowersPlaced = 0;
        totalTowersUpgraded = 0;
        totalMoneySpent = 0;
    }


    public void IncreaseDeaths(int amount = 1)
    {
        deaths += amount;
        totalDeaths += amount;
    }

    public void IncreaseKills(int amount = 1)
    {
        kills += amount;
        totalKills += amount;
    }

    public void IncreaseTowersPlaced(int amount = 1)
    {
        towersPlaced += amount;
        totalTowersPlaced += amount;
    }

    public void IncreaseTowersUpgraded(int amount = 1)
    {
        towersUpgraded += amount;
        totalTowersPlaced += amount;
    }

    public void IncreaseMoneySpent(int amount = 1)
    {
        moneySpent += amount;
        totalMoneySpent += amount;
    }
}

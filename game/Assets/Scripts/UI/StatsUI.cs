using System.Linq;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    //current level
    [SerializeField] TextMeshProUGUI kills, towersPlaced, towersUpgraded, moneySpent;
    //total
    [SerializeField] TextMeshProUGUI[] totalStats;
    private void Start()
    {
        UpdateStats();
    }

    void OnEnable()//everytime the gameObject where this script is connected to will be activated the Text will get its value
    {
        UpdateStats();
    }

    private void UpdateStats()
    {
        Statistics stats = GameManager.Instance.Save.data.stats;

        kills.text = "Kills: " + stats.kills.ToString();
        towersPlaced.text = "TowersPlaced: " + stats.towersPlaced.ToString();
        towersUpgraded.text = "TowersUpgraded: " + stats.towersUpgraded.ToString();
        moneySpent.text = "MoneySpent: " + stats.moneySpent.ToString();
        if (totalStats.Count() < 4) return;//if totalStats isn't vulled completely the method will stop with running further 
        totalStats[0].text = "Total Kills: " + stats.totalKills.ToString();
        totalStats[1].text = "Total TowersPlaced: " + stats.totalTowersPlaced.ToString();
        totalStats[2].text = "Total TowersUpgraded: " + stats.totalTowersUpgraded.ToString();
        totalStats[3].text = "Total MoneySpent: " + stats.totalMoneySpent.ToString();
    }
}

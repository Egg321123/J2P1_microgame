using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI statUI;
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

        statUI.text =
            $"\nKills: {stats.kills}\n " +
            $"Towers Placed: {stats.towersPlaced}\n" +
            $"Towers Upgraded: {stats.towersUpgraded}\n" +
            $"Money Spend: {stats.moneySpent}\n\n" +

            $"Total Kills: {stats.totalKills}\n " +
            $"Total Towers Placed: {stats.totalTowersPlaced}\n" +
            $"Total Towers Upgraded: {stats.totalTowersUpgraded}\n" +
            $"Total Money Spend: {stats.totalMoneySpent}";

    }
}

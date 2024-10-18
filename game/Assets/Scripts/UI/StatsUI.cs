using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] statUIs;
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

        foreach (TextMeshProUGUI ui in statUIs)
        {
            ui.text = 
                $"\nKills: {stats.kills}\n " +
                $"Towers Placed: {stats.towersPlaced}\n" +
                $"Towers Upgraded: {stats.towersUpgraded}\n" +
                $"Money Spend: {stats.moneySpent}\n\n" +

                $"Total Kills: {stats.totalKills}\n " +
                $"Total Towers Placed: {stats.totalTowersPlaced}\n" +
                $"Total Towers Upgraded: {stats.totalTowersUpgraded}\n" +
                $"Total Money Spend: {stats.totalMoneySpent}" ;

           /* ui.text = string.Format(ui.text,
                    stats.kills,                // {0}
                    stats.towersPlaced,         // {1}
                    stats.towersUpgraded,       // {2}
                    stats.moneySpent,           // {3}
                    stats.totalKills,           // {4}
                    stats.totalTowersPlaced,    // {5}
                    stats.totalTowersUpgraded,  // {6}
                    stats.totalMoneySpent       // {7}
            );*/
        }
    }
}

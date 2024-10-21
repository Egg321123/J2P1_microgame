using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    private bool ignoreNextCall = false;

    [SerializeField] TextMeshProUGUI[] statUIs;
    private void Start()
    {
        UpdateStats();
    }
    private void OnEnable() // everytime the gameObject where this script is connected to will be activated the Text will get its value
    {
        UpdateStats();
    }

    public void UpdateStats(bool ignoreNextCall = false)
    {
        // HACK: ignore the next call when updating statistics, so we can update before statistics are reset.
        //  but we don't update the statistics *again*.
        if (this.ignoreNextCall == true)
        {
            this.ignoreNextCall = ignoreNextCall;
            return; // ignore the call
        }

        this.ignoreNextCall = ignoreNextCall;
        Statistics stats = GameManager.Instance.Save.data.stats;

        foreach (TextMeshProUGUI ui in statUIs)
        {
            ui.text =
                $"\nTotal Deaths: {stats.deaths}\n " +
                $"Kills: {stats.kills}\n " +
                $"Towers Placed: {stats.towersPlaced}\n" +
                $"Money Spend: {stats.moneySpent}\n\n" +

                $"Total Deaths: {stats.totalDeaths}\n " +
                $"Total Kills: {stats.totalKills}\n " +
                $"Total Towers Placed: {stats.totalTowersPlaced}\n" +
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

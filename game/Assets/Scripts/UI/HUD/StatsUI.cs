using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    private bool ignoreNextCall = false;

    [SerializeField] TextMeshProUGUI statUI;
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

        statUI.text =
                $"<size=75>Current level:</size>\n" +
                $"Deaths: {StringUtils.FormatNumber(stats.deaths)}\n" +
                $"Kills: {StringUtils.FormatNumber(stats.kills)}\n" +
                $"Towers Placed: {StringUtils.FormatNumber(stats.towersPlaced)}\n" +
                $"Money Spend: {StringUtils.FormatNumber(stats.moneySpent)}\n" +

                $"\n<size=75>Total:</size>\n" +
                $"Deaths: {StringUtils.FormatNumber(stats.totalDeaths)}\n" +
                $"Kills: {StringUtils.FormatNumber(stats.totalKills)}\n" +
                $"Towers Placed: {StringUtils.FormatNumber(stats.totalTowersPlaced)}\n" +
                $"Money Spend: {StringUtils.FormatNumber(stats.totalMoneySpent)}";
    }
}

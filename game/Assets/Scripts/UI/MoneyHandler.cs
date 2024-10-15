using TMPro;
using UnityEngine;

public class MoneyHandler : MonoBehaviour
{
    [SerializeField] TMP_Text scoreUI;
    [SerializeField] long money;
    private string[] suffixes = { "", "K", "M", "B", "T", "Q" };

    private void Start()
    {
        money = GameManager.Instance.Save.data.money;
        UpdateUI();
    }

    /// <summary>
    /// subtracts the amount of money you have from the total amount
    /// </summary>
    /// <param name="price"></param>
    /// <returns>returns true if you can pay for it, false if you cannot</returns>
    public bool Pay(int price)
    {
        if (money >= price)
        {
            money -= price;
            
            GameManager.Instance.Save.data.money = money;
            GameManager.Instance.Save.data.stats.IncreaseMoneySpent(price);

            UpdateUI();
            return true;
        }
        else return false;
    }

    /// <summary>
    /// increases the amount of money you have based on the amount an enemy awards
    /// </summary>
    /// <param name="amount"></param>
    public void Earn(int amount)
    {
        money += amount;

        GameManager.Instance.Save.data.money = money;

        UpdateUI();
    }

    void UpdateUI()
    {
        scoreUI.text = MoneyFormatting();
    }

    string MoneyFormatting()
    {
        double tempMoney = money;

        // Define the size suffixes for thousand, million, billion, trillion, etc.
        int suffixIndex = 0;

        // Keep dividing money by 1000 until it is less than 1000, tracking the suffix
        while (tempMoney >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            tempMoney /= 1000;
            suffixIndex++;
        }

        // Format the number with the appropriate suffix and return
        return tempMoney.ToString("0.##") + suffixes[suffixIndex];
    }
}
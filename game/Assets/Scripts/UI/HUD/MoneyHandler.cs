using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class MoneyHandler : MonoBehaviour
{
    [SerializeField] TMP_Text scoreUI;
    private string[] suffixes = { "", "K", "M", "B", "T", "Q" };
    [SerializeField] StoreToggle[] toggles;
    private Shop shop = null;

    private void Awake()
    {
        shop = FindFirstObjectByType<Shop>();
    }

    private void FixedUpdate() => scoreUI.text = MoneyFormatting();

    /// <summary>
    /// subtracts the amount of money you have from the total amount
    /// </summary>
    /// <param name="price"></param>
    /// <returns>returns true if you can pay for it, false if you cannot</returns>
    public bool Pay(TowerStoreData tower)
    {
        int price = tower.ScaledCost;
        Save save = GameManager.Instance.Save;

        if (GameManager.Instance.Save.data.money >= price)
        {
            save.data.money -= price;
            save.data.stats.IncreaseMoneySpent(price);
            save.data.towerBoughtCount[(int)tower.towerData.towerType]++; // increase the amount bought from this type
            shop.UpdateStore();

            return true;
        }
        else return false;
    }

    /// <summary>
    /// increases the amount of money you have based on the amount an enemy awards
    /// </summary>
    /// <param name="amount"></param>
    public void Earn(int amount) => GameManager.Instance.Save.data.money += amount;

    string MoneyFormatting()
    {
        double tempMoney = GameManager.Instance.Save.data.money;

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
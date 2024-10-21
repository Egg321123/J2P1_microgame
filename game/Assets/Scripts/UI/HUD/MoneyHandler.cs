using TMPro;
using UnityEngine;

public class MoneyHandler : MonoBehaviour
{
    [SerializeField] TMP_Text scoreUI;
    [SerializeField] StoreToggle[] toggles;
    private Shop shop = null;

    private void Awake()
    {
        shop = FindFirstObjectByType<Shop>();
    }

    private void FixedUpdate() => scoreUI.text = StringUtils.FormatNumber(GameManager.Instance.Save.data.money);

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
}

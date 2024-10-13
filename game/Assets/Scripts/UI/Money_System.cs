using TMPro;
using UnityEngine;

public class MoneyHandler : MonoBehaviour
{
    [SerializeField] TMP_Text scoreUI;
    [SerializeField] int money;

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

    void UpdateUI() => scoreUI.text = money.ToString();
}

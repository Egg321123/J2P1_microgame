using TMPro;
using UnityEngine;

public class Money_System : MonoBehaviour
{
    [SerializeField] TMP_Text scoreUI;
    [SerializeField] int money = 50;
    /*[SerializeField] int cost = 10;
    [SerializeField] int give = 20;*/
    //int moneyMin = 0;
    /*private void FixedUpdate()
    {
        UpdateMoney();
        if (money <= 0)
        {
            money = moneyMin;
        }
    }*/
    void UpdateUI() => scoreUI.text = money.ToString();
    public bool Pay(int price)
    {
        if (money >= price)
        {
            money -= price;
            UpdateUI();
            return true;
        }
        else return false;
        /* if (money <= cost) 
         {
             Debug.Log("Not enough money");
         }*/
    }
    public void Earn(int amaunt)
    {
        money += amaunt;
        UpdateUI();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] TMP_Text scoreUI;
    [SerializeField] int cost = 10;
    [SerializeField] int give = 20;
    int money = 50;
    int moneyMin = 0;
    private void FixedUpdate()
    {
        UpdateMoney();
        if (money <= 0)
        {
            money = moneyMin;
        }
    }
    void UpdateMoney()
    {
        scoreUI.text = money.ToString();
    }

    public void OnClickDelete()
    {
        if(money >= cost)
        {
            money -= cost;
        }
        if (money <= cost) 
        {
            Debug.Log("Not enough money");
        }
    }
    public void OnClickGive() 
    {
        money += give;
    }
}

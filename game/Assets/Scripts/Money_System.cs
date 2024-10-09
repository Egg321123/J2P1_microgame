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
    [SerializeField] int money = 50;
    [SerializeField] int moneyMin = 0;
    public int goblinEarn1 = 3;
    public int goblinEarn2 = 5;
    public int goblinEarn3 = 10;
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
    public void goblin1()
    {
        money += goblinEarn1;
    }
    public void goblin2()
    {
        money += goblinEarn2;
    }
    public void goblin3()
    {
        money += goblinEarn3;
    }
}

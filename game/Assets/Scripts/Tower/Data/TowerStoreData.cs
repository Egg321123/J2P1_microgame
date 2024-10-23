using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Default", menuName = "Shop item (Tower)")]
public class TowerStoreData : ScriptableObject
{
    //this scriptable object is for use in the store, not to store the values for saving system

    [Header("Store specific values")]
    public string towerName = "Default";
    [TextArea] public string towerInfo = "Default";
    public Sprite menuSprite;
    public int cost = 1;

    // gets the cost scaled with how many times the tower has bought
    public int ScaledCost
    {
        get
        {
            Save save = GameManager.Instance.Save;
            TowerType type = towerData.towerType;
            return (int)MathF.Round(cost * MathF.Pow(1.3F, save.data.towerBoughtCount[(int)type]));
        }
    }

    [Header("Base tower information")]
    public TowerData towerData = new()
    {
        towerType = TowerType.ARROW_TOWER,
        attackSpeed = 0.5f,
        attackRadius = 5,
        attackDamage = 1,
        targetCount = 1,
        projectileSpeed = 2,
    };
}

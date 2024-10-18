using UnityEngine;

[CreateAssetMenu(fileName = "Default", menuName = "Shop item (Tower)")]
public class TowerStoreData : ScriptableObject
{
    //this scriptable object is for use in the store, not to store the values for saving system

    [Header("Store specific values")]
    public string towerName = "Default";
    [TextArea]public string towerInfo = "Default";
    public Sprite menuSprite;
    public int cost = 1;

    [Header("Base tower information")]
    public TowerData towerData = new()
    {
        towerType = TowerType.ARROW_TOWER,
        attackSpeed = 0.5f,
        attackRange = 5,
        attackDamage = 1,
        projectileSpeed = 2,
        level = 1
    };
}

using UnityEngine;

[CreateAssetMenu(fileName = "Default", menuName = "Shop item (Tower)")]
public class TowerStoreData : ScriptableObject
{
    //this scriptable object is for use in the store, not to store the values for saving system

    [Header("Store specific values")]
    public string towerName;
    public Texture menuImg;
    public int cost;

    [Header("Base tower information")]
    public TowerData towerData;

}

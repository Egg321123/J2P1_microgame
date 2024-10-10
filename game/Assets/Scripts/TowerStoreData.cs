using UnityEngine;

[CreateAssetMenu(fileName = "Default", menuName = "Shop item (Tower)")]
public class TowerStoreData : ScriptableObject
{
    //this scriptable object is for use in the store,
    //this has to be converted to a struct when passing it to the tower,
    //as scriptable objects aren't meant to be written to, thus can't be used in the saving system.

    public string towerName;
    public GameObject tower;
    public int cost;

    public float attackSpeed;
    public float projectileSpeed;
    public float attackRange;
    public float attackDamage;

}

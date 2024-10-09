using UnityEngine;

[CreateAssetMenu(fileName = "Default Tower", menuName = "Tower Object")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public GameObject tower;
    public int cost;
    public int level;

    public float attackSpeed;
    public float attackRange;
    public float attackDamage;
}

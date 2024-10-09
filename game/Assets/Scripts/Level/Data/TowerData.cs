using UnityEngine;

public struct TowerData
{
    public GameObject tower;
    public int level;


    public TowerData(GameObject tower, int level = 1)
    {
        this.tower = tower;
        this.level = level;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTowerType : MonoBehaviour
{
    [SerializeField] TowerBase tower;
    [SerializeField] TowerStoreData data;

    // Start is called before the first frame update
    void Start()
    {
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        tower.TowerData = data.towerData;
    }
}

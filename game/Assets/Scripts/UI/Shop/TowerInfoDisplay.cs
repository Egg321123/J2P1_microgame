using UnityEngine;
using TMPro;

public class TowerInfoDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text towerName;
    [SerializeField] TMP_Text towerDescription;

    public void UpdateInformation(TowerStoreData data)
    {
        towerName.text = data.towerName;
        towerDescription.text = data.towerInfo;
    }
}

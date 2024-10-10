using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoreToggle : MonoBehaviour
{
    [SerializeField] TMP_Text topText;
    [SerializeField] TMP_Text bottomText;

    [SerializeField] Image BackroundImg;
    [SerializeField] Image ToggledImg;


    public void SetButtonValues(TowerStoreData data)
    {
        topText.text = data.towerName;
        bottomText.text = data.cost.ToString();

        BackroundImg.sprite = data.menuSprite;
        ToggledImg.sprite = data.menuSprite;
    }
}

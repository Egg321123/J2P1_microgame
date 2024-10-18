using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StoreToggle : MonoBehaviour
{
    [SerializeField] TMP_Text bottomText;
    [SerializeField] Image Img;


    public void SetButtonValues(TowerStoreData data)
    {
        bottomText.text = data.cost.ToString();

        Img.sprite = data.menuSprite;
    }

    public void UpdateAvailable(bool isAvailable)
    {
        Img.color = isAvailable ? Color.white : Color.grey;
    }
}

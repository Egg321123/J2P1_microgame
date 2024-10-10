using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    TowerStoreData[] towers = new TowerStoreData[4];
    [SerializeField] List<TowerStoreData> itemData = new List<TowerStoreData>();
    [SerializeField] List<GameObject> toggles = new List<GameObject>();
    private PlaceOnGrid place;
    void Start()
    {
        LoadNewShop();
        place = FindFirstObjectByType<PlaceOnGrid>();
    }
    void LoadShop()
    {
        for (int toggle = 0; toggle < toggles.Count; toggle++)
        {
            Transform gOText = toggles[toggle].transform.GetChild(0);
            for (int text = 0; text < gOText.childCount; text++)
            {
                string data;
                if (text == 0) data = towers[toggle].towerName;
                else data = "Price: " + towers[toggle].cost.ToString();
                gOText.GetChild(text).GetComponent<TextMeshProUGUI>().text = data;
            }
            //toggles[toggle].transform.GetChild(1).GetComponent<Image>().sprite = tItemData[randomItem].menuImg;
            //tImages.RemoveAt(randomItem);
        }
    }
    public void LoadNewShop()
    {
        List<TowerStoreData> tItemData = new List<TowerStoreData>();
        foreach (var data in itemData) tItemData.Add(data);

        for (int toggle = 0; toggle < toggles.Count; toggle++)
        {
            Transform gOText = toggles[toggle].transform.GetChild(0);
            int randomItem = Random.Range(0, tItemData.Count);
            for (int text = 0; text < gOText.childCount; text++)
            {
                string data;
                if (text == 0) data = tItemData[randomItem].towerName;
                else data = "Price: " + tItemData[randomItem].cost.ToString();
                gOText.GetChild(text).GetComponent<TextMeshProUGUI>().text = data;
            }
            towers[toggle] = tItemData[randomItem];
            //toggles[toggle].transform.GetChild(1).GetComponent<Image>().sprite = tItemData[randomItem].menuImg;
            //tImages.RemoveAt(randomItem);
            tItemData.RemoveAt(randomItem);
        }
    }
    public void item1() => place.PlaceModeToggle(towers[0]);
    public void item2() => place.PlaceModeToggle(towers[1]);
    public void item3() => place.PlaceModeToggle(towers[2]);
    public void item4() => place.PlaceModeToggle(towers[3]);
}
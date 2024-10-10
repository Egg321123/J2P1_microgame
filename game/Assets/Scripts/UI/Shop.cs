using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    string[] towers = new string[4];
    [SerializeField] List<TowerStoreData> itemData = new List<TowerStoreData>();
    [SerializeField] List<GameObject> toggles = new List<GameObject>();
    private PlaceOnGrid place;

    void Start()
    {
        LoadShop();
        place = FindFirstObjectByType<PlaceOnGrid>();
    }
    public void LoadShop()
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
            towers[toggle] = tItemData[randomItem].towerName;
            //toggles[toggle].transform.GetChild(1).GetComponent<Image>().sprite = tItemData[randomItem].menuImg;
            //tImages.RemoveAt(randomItem);
            tItemData.RemoveAt(randomItem);
        }
    }
    TowerStoreData FindTower(string tower2Search)
    {
        for (int tower = 0; tower < itemData.Count; tower++) if (itemData[tower].towerName == tower2Search) return itemData[tower];
        return null;
    }
    public void item1() => place.PlaceModeToggle(FindTower(towers[0]));
    public void item2() => place.PlaceModeToggle(FindTower(towers[1]));
    public void item3() => place.PlaceModeToggle(FindTower(towers[2]));
    public void item4() => place.PlaceModeToggle(FindTower(towers[3]));
}
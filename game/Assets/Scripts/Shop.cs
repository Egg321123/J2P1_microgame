using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    string[] towers = new string[4];
    [SerializeField] List<TowerData> itemData = new List<TowerData>();
    [SerializeField] List<Sprite> images = new List<Sprite>();
    [SerializeField] List<GameObject> toggles = new List<GameObject>();
    [SerializeField] Money_System money;
    //[SerializeField] List<TextMeshPro> itemInfo;
    // Start is called before the first frame update
    void Start()
    {
       LoadShop();
        money = GetComponentInParent<Money_System>();
    }
    void LoadShop()
    {
        List<TowerData> tItemData = new List<TowerData>();
        List<Sprite> tImages = new List<Sprite>();
        foreach (var data in itemData) tItemData.Add(data);
        foreach (var sprite in images) tImages.Add(sprite);

        for (int toggle = 0; toggle < toggles.Count; toggle++)
        {
            Transform gOText = toggles[toggle].transform.GetChild(0);
            // itemInfo = new List<TextMeshPro>(buttons[button].GetComponentInChildren<TextMeshPro>());
            //List<> itemData = new List<>();
            int randomItem = Random.Range(0, tItemData.Count);
            for (int text = 0; text < gOText.childCount; text++)
            {
                string data;
                if (text == 0) data = tItemData[randomItem].name;
                else data = "Price: " + tItemData[randomItem].cost.ToString();
                gOText.GetChild(text).GetComponent<TextMeshProUGUI>().text = data;
            }
            towers[toggle] = tItemData[randomItem].name;
            toggles[toggle].transform.GetChild(1).GetComponent<Image>().sprite = tImages[randomItem];
            tImages.RemoveAt(randomItem);
            tItemData.RemoveAt(randomItem);
        }
    }
    TowerData FindTower(string tower2Search)
    {
        for ( int tower = 0; tower < itemData.Count; tower++) if (itemData[tower].name == tower2Search) return itemData[tower];
        return null;
    }
    public void item1()
    {
        TowerData tower = FindTower(towers[0]);
        if (money.Pay(tower.cost)) FindFirstObjectByType<PlaceOnGrid>().PlaceModeToggle(tower);
    }
    public void item2()
    {
        FindFirstObjectByType<PlaceOnGrid>().PlaceModeToggle(FindTower(towers[1]));

    }
    public void item3()
    {
        FindFirstObjectByType<PlaceOnGrid>().PlaceModeToggle(FindTower(towers[2]));

    }
    public void item4()
    {
        FindFirstObjectByType<PlaceOnGrid>().PlaceModeToggle(FindTower(towers[3]));

    }
}
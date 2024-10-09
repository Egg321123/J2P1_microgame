using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    string[] towers = new string[4];
    [SerializeField] List<PlaceHolderItmes> itemData = new List<PlaceHolderItmes>();
    [SerializeField] List<Sprite> images = new List<Sprite>();
    [SerializeField] List<GameObject> toggles = new List<GameObject>();
    //[SerializeField] List<TextMeshPro> itemInfo;
    // Start is called before the first frame update
    void Start()
    {
        List<PlaceHolderItmes> tItemData = itemData;
        List<Sprite> tImages = images;
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
                else data = "Price: " + tItemData[randomItem].price.ToString();
                gOText.GetChild(text).GetComponent<TextMeshProUGUI>().text = data;
            }
            towers[randomItem] = tItemData[randomItem].name;
            toggles[toggle].transform.GetChild(1).GetComponent<Image>().sprite = tImages[randomItem];
            tImages.RemoveAt(randomItem);
            tItemData.RemoveAt(randomItem);
        }
    }
    public void item1()
    {
        print("1");
    }
    public void item2()
    {
        print(2);

    }
    public void item3()
    {
        print(3);

    }
    public void item4()
    {
        print(4);

    }
}
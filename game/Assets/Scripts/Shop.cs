using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    string[] items = new string[4];
    //List<> itemData = new List<>();
    [SerializeField] List<Sprite> images = new List<Sprite>();
    [SerializeField] List<GameObject> buttons = new List<GameObject>();
    [SerializeField] List<TextMeshPro> itemInfo;
    // Start is called before the first frame update
    void Start()
    {
        for (int button = 0; button < buttons.Count; button++)
        {
            itemInfo = new List<TextMeshPro>((IEnumerable<TextMeshPro>)buttons[button].GetComponentInChildren<TextMeshPro>());
            //List<> itemData = new List<>();
            int randomItem = Random.Range(0, items.Length);
            for (int text = 0; text < itemInfo.Count; text++)
            {
                if (text == 0) itemInfo[text].text = "name";
                else itemInfo[text].text = "price: ";
            }
            buttons[button].GetComponent<Image>().sprite = images[randomItem];
        }
    }
    public void item1()
    {

    }
    public void item2()
    {

    }
    public void item3()
    {

    }
    public void item4()
    {

    }
}
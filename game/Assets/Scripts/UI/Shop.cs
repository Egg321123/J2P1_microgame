using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
public class Shop : MonoBehaviour
{
    //stores the towers you want available in the shop
    [SerializeField] TowerStoreData[] availableTowers;

    //stores the toggles for the towers
    [SerializeField] GameObject[] toggles = new GameObject[4];
    [SerializeField] ToggleGroup group;

    private TowerStoreData[] towersInShop = new TowerStoreData[4];
    private bool hasButtonPressed = false;

    //store reference to place script
    private PlaceOnGrid place;

    private void Start()
    {
        //load place, and generate new shop
        place = FindFirstObjectByType<PlaceOnGrid>();
        RegenerateStore();
    }

    public void RegenerateStore()
    {
        List<TowerStoreData> towers = availableTowers.ToList();

        //fill in the store with new random towers
        for (int i = 0; i < towersInShop.Length; i++)
        {
            //get random number that fits the tower length
            int randomIndex = Random.Range(0, towers.Count);

            //store the random tower
            towersInShop[i] = towers[randomIndex];

            //remove the random tower from the available list for this itteration
            towers.Remove(towers[randomIndex]);
        }

        //update buttons
        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].GetComponent<StoreToggle>().SetButtonValues(towersInShop[i]);
        }
    }

    public void StoreButtons(int index)
    {
        //if you have no toggles active anymore, disable place mode
        if (!group.AnyTogglesOn())
        {
            place.PlaceModeToggle(towersInShop[index]);
            hasButtonPressed = false;
        }
        else if (!hasButtonPressed)
        {
            place.PlaceModeToggle(towersInShop[index]);
            hasButtonPressed = true;
        }
        else
        {
            place.ChangeTower(towersInShop[index]);
        }
    }
}

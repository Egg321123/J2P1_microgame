using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] TowerInfoDisplay towerInfo;

    [SerializeField] private GameObject audioPrefab;
    [SerializeField] private AudioClip clip;

    //stores the towers you want available in the shop
    [SerializeField] TowerStoreData[] availableTowers;


    //stores the toggles for the towers
    [SerializeField] StoreToggle[] toggles = new StoreToggle[4];
    [SerializeField] ToggleGroup group;

    [SerializeField] GameObject shop;

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

    private void FixedUpdate()
    {
        //update the ui
        for (int i = 0; i < toggles.Length; i++)
        {
            bool available = towersInShop[i].cost <= GameManager.Instance.Save.data.money;
            toggles[i].UpdateAvailable(available);
        }
    }

    // updates the store
    public void UpdateStore()
    {
        //update buttons
        for (int i = 0; i < toggles.Length; i++) toggles[i].SetButtonValues(towersInShop[i]);
    }

    public void RegenerateStore()
    {
        /*
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
        */

        // for release we disabled randomisation because we didn't have enough towers for it to make sense and it actually disabled quality
        for (int i = 0; i < towersInShop.Length; i++) towersInShop[i] = availableTowers[i];

        UpdateStore();
    }

    public void StoreButtons(int index)
    {
        GameObject sound = Instantiate(audioPrefab, transform.position, Quaternion.identity);
        sound.GetComponent<AudioClipPlayer>().Initialize(clip);

        //if you have no toggles active anymore, disable place mode
        if (!group.AnyTogglesOn())
        {
            place.DisablePlaceMode();
            hasButtonPressed = false;
            towerInfo.gameObject.SetActive(false);
        }
        else if (!hasButtonPressed)
        {
            place.EnablePlaceMode(towersInShop[index]);
            hasButtonPressed = true;
            towerInfo.gameObject.SetActive(true);
            towerInfo.UpdateInformation(towersInShop[index]);
        }
        else
        {
            place.ChangeTower(towersInShop[index]);
            towerInfo.UpdateInformation(towersInShop[index]);
        }
    }

    public void ShopToggle(bool open)
    {
        shop.SetActive(open);
        if (!open) group.SetAllTogglesOff();
    }
}

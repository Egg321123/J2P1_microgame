using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceOnGrid : MonoBehaviour
{
    [SerializeField] private GameObject audioPrefab;
    [SerializeField] private AudioClip clip;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private GameObject grid;
    [SerializeField] private MonoTile objectToPlace = null;

    private MoneyHandler moneyHandler;
    private MonoLevel monoLevel = null;
    private bool isInPlaceMode = false;
    private GameObject gridObj = null;

    private TowerStoreData storeData;
    private TowerData towerData;

    private void Start()
    {
        monoLevel = FindFirstObjectByType<MonoLevel>();
        moneyHandler = FindFirstObjectByType<MoneyHandler>();
        CreateGrid();
    }

    public void EnablePlaceMode(TowerStoreData data)
    {
        storeData = data;
        towerData = storeData.towerData;

        isInPlaceMode = true;
        gridObj.SetActive(true);

        //go into the place mode cycle
        StartCoroutine(PlacingMode());
    }

    public void DisablePlaceMode()
    {
        isInPlaceMode = false;
        gridObj.SetActive(false);
    }

    public void ChangeTower(TowerStoreData data)
    {
        storeData = data;
        towerData = storeData.towerData;
    }

    private IEnumerator PlacingMode()
    {
        while (isInPlaceMode)
        {
            // ignore if the touch count is anything but 1
            if (Input.touchCount != 1)
            {
                yield return null;
                continue;
            }

            // grab only the first finger touching the screen and continue if it's not the first touch
            Touch touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began)
            {
                yield return null;
                continue;
            }

            // check the next 5 frames whether the user moved, don't place a tower if so
            bool placeTower = true;
            for (int i = 0; i < 5; i++)
            {
                yield return null;
                if (Input.touchCount == 0) break;

                Touch checkTouch = Input.GetTouch(0);
                if (checkTouch.phase == TouchPhase.Ended) break;
                if (checkTouch.phase == TouchPhase.Moved)
                {
                    placeTower = false;
                    break;
                }
            }

            // skip if we don't place a tower, or are clicking on the ui instead of scene
            if (placeTower == false || EventSystem.current.IsPointerOverGameObject(0))
            {
                yield return null;
                continue;
            }

            //creates the ray with proper parameters
            Ray ray = Camera.main.ScreenPointToRay(new(touch.position.x, touch.position.y, 0));

            // Does the raycast check
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, targetLayer))
            {
                // Gets the world position
                Vector2Int pos = new((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.z));

                // Places object in the scene if you are allowed to place there
                if (monoLevel.Level.IsEmpty(pos) && moneyHandler.Pay(storeData.ScaledCost))
                {
                    GameObject sound = Instantiate(audioPrefab, transform.position, Quaternion.identity);
                    sound.GetComponent<AudioClipPlayer>().Initialize(clip, 20, true);
                    GameManager.Instance.Save.data.stats.IncreaseTowersPlaced();
                    monoLevel.SetTile(objectToPlace, pos, TileType.TOWER, towerData, true);
                }
            }

            yield return null;
        }

        yield return null;
    }

    /// <summary>
    /// creates the grid needed for placing on the grid.
    /// </summary>
    private void CreateGrid()
    {
        Level level = monoLevel.Level;

        //temporarly setting manually, will use grid size of level generator later
        int width = level.width;
        int height = level.height;

        //spawn in object
        gridObj = Instantiate(grid, Vector3.zero, Quaternion.identity);

        //set scale to fit grid size
        gridObj.transform.localScale = new(width, .01f, height);

        //parent the object to this object, and hide it
        gridObj.transform.parent = transform;
        gridObj.SetActive(false);
    }
}

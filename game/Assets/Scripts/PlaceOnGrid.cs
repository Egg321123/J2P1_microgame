using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlaceOnGrid : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private GameObject grid;
    [SerializeField] private MonoTile objectToPlace = null;

    private MonoLevel monoLevel = null;
    private bool isInPlaceMode = false;
    private GameObject gridObj = null;

    private TowerData towerData;

    private void Start()
    {
        StartCoroutine(WaitForLevel());
    }
    IEnumerator WaitForLevel()
    {
        while (monoLevel == null)
        {
            monoLevel = FindFirstObjectByType<MonoLevel>();
            yield return null;
        }

        //create grid only when monolevel has become available
        CreateGrid();

        yield return null;
    }

    public void PlaceModeToggle(TowerStoreData data)
    {
        towerData = data.towerData;

        isInPlaceMode = !isInPlaceMode;
        gridObj.SetActive(!gridObj.activeInHierarchy);

        //go into the place mode cycle
        StartCoroutine(PlacingMode());
    }

    public void ChangeTower(TowerStoreData data)
    {
        towerData = data.towerData;
    }

    private IEnumerator PlacingMode()
    {
        while (isInPlaceMode)
        {
            //check if screen has been touched
            if (Input.touchCount > 0)
            {
                //grab only the first finger touching the screen
                Touch touch = Input.GetTouch(0);

                //avoids race condition from ui toggle (takes longer to toggle then to send raycast)
                yield return new WaitForSeconds(0.1f);
                if (!isInPlaceMode) yield break;

                //when it registers the touch from the screen
                if (touch.phase == TouchPhase.Began)
                {
                    //creates the ray with proper parameters
                    Ray ray = Camera.main.ScreenPointToRay(new(touch.position.x, touch.position.y, 0));

                    // Does the raycast check
                    if (Physics.Raycast(ray, out RaycastHit hit, 1000f, targetLayer))
                    {
                        // Gets the world position
                        Vector2Int pos = new((int)Mathf.Floor(hit.point.x), (int)Mathf.Floor(hit.point.z));

                        // Places object in the scene if you are allowed to place there
                        if (monoLevel.Level.IsEmpty(pos)) monoLevel.SetTile(objectToPlace, pos, TileType.TOWER, towerData, true);
                    }

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

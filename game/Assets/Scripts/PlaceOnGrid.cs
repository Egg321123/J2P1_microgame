using System;
using System.Collections;
using UnityEngine;

public class PlaceOnGrid : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private GameObject grid;

    private MonoLevel level = null;
    private bool isInPlaceMode = false;
    private GameObject gridObj = null;
    private MonoTile objectToPlace = null;

    private void Start()
    {
        level = FindFirstObjectByType<MonoLevel>();
        CreateGrid();
    }

    public void PlaceModeToggle(MonoTile pObjectToPlace = null)
    {
        //update the object the user wants to place
        objectToPlace = pObjectToPlace; 

        isInPlaceMode = !isInPlaceMode;
        gridObj.SetActive(!gridObj.activeInHierarchy);

        //go into the place mode cycle
        StartCoroutine(PlacingMode(objectToPlace));
    }

    public void ChangeTower(MonoTile pObjectToPlace)
    {
        //update the object the user wants to place
        objectToPlace = pObjectToPlace;
    }


    private IEnumerator PlacingMode(MonoTile placeObject)
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
                        Vector3 worldPos = hit.point;

                        // Snaps to the nearest 0.5f tile center
                        worldPos.x = Mathf.Floor(worldPos.x) + 0.5f;
                        worldPos.y = 0;  // Assuming you want the y to be fixed
                        worldPos.z = Mathf.Floor(worldPos.z) + 0.5f;

                        // Creates the quaternion for rotation
                        Quaternion rot = Quaternion.Euler(new Vector3(-90, 0, 0));

                        // Places object in the scene
                        MonoTile tile = Instantiate(placeObject, worldPos, rot);

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
        Level level = FindFirstObjectByType<MonoLevel>().Level;

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

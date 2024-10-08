using System;
using System.Collections;
using UnityEngine;

public class PlaceOnGrid : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private GameObject grid;

    private bool isInPlaceMode = false;
    private GameObject gridObj = null;
    private GameObject objectToPlace = null;

    private void Start() => CreateGrid();

    public void PlaceModeToggle(GameObject pObjectToPlace = null)
    {
        //update the object the user wants to place
        objectToPlace = pObjectToPlace; 

        isInPlaceMode = !isInPlaceMode;
        gridObj.SetActive(!gridObj.activeInHierarchy);

        //go into the place mode cycle
        StartCoroutine(PlacingMode(objectToPlace));
    }

    public void ChangeTower(GameObject pObjectToPlace)
    {
        //update the object the user wants to place
        objectToPlace = pObjectToPlace;
    }


    private IEnumerator PlacingMode(GameObject placeObject)
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

                    //does the raycast check
                    if (Physics.Raycast(ray, out RaycastHit hit, 1000f, targetLayer))
                    {
                        //gets the world position
                        Vector3 worldPos = hit.point;

                        //rounds down on the .5 instead of using casting, which always rounds it in one way regardless of how high or low the decimal is
                        worldPos.x = (float)Math.Round(worldPos.x) + 0.5f;
                        worldPos.y = 0;
                        worldPos.z = (float)Math.Round(worldPos.z) + 0.5f;

                        //creates quat rot
                        Quaternion rot = Quaternion.Euler(new(-90, 0, 0));

                        //places object in scene
                        Instantiate(placeObject, worldPos, rot);
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

        //edit texture scale to fit with grid size
        Material mat = gridObj.GetComponent<MeshRenderer>().material;
        mat.mainTextureScale = new(height, width);

        //parent the object to this object, and hide it
        gridObj.transform.parent = transform;
        gridObj.SetActive(false);
    }
}
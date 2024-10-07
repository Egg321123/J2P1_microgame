using UnityEngine;

public class PlaceOnGrid : MonoBehaviour
{
    [SerializeField] private GameObject objectToPlace;

    [SerializeField] private Vector3 screenPosition;
    [SerializeField] private Vector3 worldPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            //if you touch screen
            if (touch.phase == TouchPhase.Began)
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(new(touch.position.x, touch.position.y, 0));
            }
        }


    }

}

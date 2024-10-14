using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public const float TAU = MathF.PI * 2; // τ > π

    [SerializeField] private float minAmount = 2.2F;
    [SerializeField] private float maxAmount = TAU / 2.0F;
    [SerializeField] private float radius = 25.0F;

    private float amount = 3.0F;
    private int lvlWidth = 0;
    private int lvlHeight = 0;

    private void GetInput(int fingers)
    {
        switch (fingers)
        {
            case 1:
                Touch touch = Input.GetTouch(0);
                float moveAmount = touch.deltaPosition.y / Screen.height * TAU; // get the amount that should be moved
                amount -= moveAmount;
                break;
            case 2:
                Touch t1 = Input.GetTouch(0);
                Touch t2 = Input.GetTouch(1);

                // get where the finger was last frame
                Vector2 t1d = t1.position - t1.deltaPosition;
                Vector2 t2d = t2.position - t2.deltaPosition;

                // get the lengths of the current and previous, and get the difference between these two
                float direction = (t1.position - t2.position).magnitude - (t1d - t2d).magnitude;
                radius -= direction / Screen.width * radius;    // apply the difference to the radius
                break;
        }


    }

    private Vector3 GetCameraPosition()
    {
        float z = MathF.Sin(amount + (TAU / 2.0F)) * radius;
        float y = MathF.Cos(amount + (TAU / 2.0F)) * radius;

        return new Vector3(lvlWidth / 2.0F, y, (lvlHeight / 2.0F) + z);
    }

    // start is called before the first frame update
    private void Start()
    {
        Level level = FindFirstObjectByType<MonoLevel>().Level;
        lvlWidth = level.width;
        lvlHeight = level.height;
    }

    // update is called every frame
    private void Update()
    {
        if (Input.touchCount < 0)
            return;

        GetInput(Input.touchCount);

        // set the camera position
        transform.position = GetCameraPosition();
        transform.LookAt(new Vector3(lvlWidth / 2.0F, 0.0F, lvlHeight / 2.0F));
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(new Vector3(lvlWidth / 2.0F, 0.0F, lvlHeight / 2.0F), radius);
    }
#endif
}

using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public const float TAU = MathF.PI * 2; // τ > π

    [Header("camera tilt (radians)")]
    [SerializeField, Range(0.0F, TAU)] private float tiltAmount = 3.0F;             // the amount that the camera is tilted
    [SerializeField, Range(0.0F, TAU)] private float minTiltAmount = 2.2F;          // the minimum amount that the camera is allowed to be tilted
    [SerializeField, Range(0.0F, TAU)] private float maxTiltAmount = TAU / 2.0F;    // the maximum amount that the camera is allowed to be tilted

    [Header("camera zoom (radius)")]
    [SerializeField, Min(0)] private float radius = 25.0F;                          // the radius of the circle on which the camera moves
    [SerializeField, Min(0)] private float minRadius = 10.0F;                       // the mimimum radius that the circle is allowed to have
    [SerializeField, Min(0)] private float maxRadius = 35.0F;                       // the maximum radius that the circle is allowed to have

    // level dimensions
    private int lvlWidth = 0;
    private int lvlHeight = 0;

    // updates the input variables
    private void UpdateInput(int fingers)
    {
        switch (fingers)
        {
            case 1:
                Touch touch = Input.GetTouch(0);
                float moveAmount = touch.deltaPosition.y / Screen.height * TAU;                     // get the amount that should be tilted
                tiltAmount = Mathf.Clamp(tiltAmount - moveAmount, minTiltAmount, maxTiltAmount);    // apply the tilt amount, clamping the value between the min and max
                break;
            case 2:
                Touch t1 = Input.GetTouch(0);
                Touch t2 = Input.GetTouch(1);

                // get where the finger was last frame
                Vector2 t1d = t1.position - t1.deltaPosition;
                Vector2 t2d = t2.position - t2.deltaPosition;

                float direction = (t1.position - t2.position).magnitude - (t1d - t2d).magnitude;    // get the lengths of the current and previous, and get the difference between these two
                float amount = direction / Screen.width * radius;                                   // calculate the amount that the radius should change
                radius = Mathf.Clamp(radius - amount, minRadius, maxRadius);                        // apply the amount to the radius, clamping the value between minimum radius and maximum radius
                break;
        }
    }

    private Vector3 GetCameraPosition()
    {
        float z = MathF.Sin(tiltAmount + (TAU / 2.0F)) * radius;
        float y = MathF.Cos(tiltAmount + (TAU / 2.0F)) * radius;

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

        // update the input
        UpdateInput(Input.touchCount);

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

using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // tau is a better mathematical constant to use than pi, because pi is only half a circle in radians, instead of a full circle.
    // in the various formulas it's thus more intuitive to use tau.
    public const float TAU = MathF.PI * 2.0F;   // τ > π

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

    // the X position of the camera
    private float cameraPosX = 0.0F;

    // checks whether the finger is moving
    private bool IsMoving(Touch finger)
    {
        return finger.phase == TouchPhase.Moved;
    }

    // updates the input variables
    private void UpdateInput(int fingers)
    {
        // switch for the amount of fingers that are touching the screen :3
        switch (fingers)
        {
            // if only one finger is touching the screen
            case 1:
                Touch touch = Input.GetTouch(0);                                                    // get the finger

                // ignore if it's not moving
                if (IsMoving(touch) == false)
                    return;

                // tilt movement
                float moveAmountY = touch.deltaPosition.y / Screen.height * TAU;                    // get the amount that should be tilted
                tiltAmount = Mathf.Clamp(tiltAmount - moveAmountY, minTiltAmount, maxTiltAmount);   // apply the tilt amount, clamping the value between the min and max

                // side-to-side movement (adding 1 to lvlWidth to include the *whole* tile, rather than stopping one before)
                float moveAmountX = touch.deltaPosition.x / Screen.width * (lvlWidth + 1) * 0.5F;   // multiplying by 0.5, because 1.0 is too fast
                cameraPosX = Mathf.Clamp(cameraPosX - moveAmountX, 0.0F, lvlWidth + 1);
                break;

            // if two fingers are touching the screens
            case 2:
                // get the fingers
                Touch t1 = Input.GetTouch(0);
                Touch t2 = Input.GetTouch(1);

                // ignore if it's not moving
                if (IsMoving(t1) == false || IsMoving(t2) == false)
                    return;

                // get where the finger was last frame
                Vector2 t1d = t1.position - t1.deltaPosition;
                Vector2 t2d = t2.position - t2.deltaPosition;

                float direction = (t1.position - t2.position).magnitude - (t1d - t2d).magnitude;    // get the lengths of the current and previous, and get the difference between these two
                float amount = direction / Screen.width * radius;                                   // calculate the amount that the radius should change
                radius = Mathf.Clamp(radius - amount, minRadius, maxRadius);                        // apply the amount to the radius, clamping the value between minimum radius and maximum radius
                break;
        }
    }

    // get the camera position from the radius and tiltAmount where it falls on a circle on a ZY plane
    private Vector3 GetCameraPosition()
    {
        // calculate the Z and Y position of the camera
        // shifting the amount by half a circle, as I found it more easy number-wise to get values you want
        float z = MathF.Sin(tiltAmount + (TAU / 2.0F)) * radius;
        float y = MathF.Cos(tiltAmount + (TAU / 2.0F)) * radius;

        // return the camera position, relative to the level's dimensions (assuming level is rendered from 0,0 towards the positive axes)
        return new Vector3(cameraPosX, y, (lvlHeight / 2.0F) + z);
    }

    // start is called before the first frame update
    private void Start()
    {
        // get the monolevel and extract the level dimensions
        Level level = FindFirstObjectByType<MonoLevel>().Level;
        lvlWidth = level.width;
        lvlHeight = level.height;
        cameraPosX = lvlWidth / 2.0F;
    }

    // update is called every frame
    private void Update()
    {
        // if no fingers are touching, return
        if (Input.touchCount < 0)
            return;

        // get the latest data from the input
        UpdateInput(Input.touchCount);

        // set the camera position
        transform.position = GetCameraPosition();
        transform.LookAt(new Vector3(cameraPosX, 0.0F, lvlHeight / 2.0F));
    }

#if UNITY_EDITOR // debugging utility
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(new Vector3(lvlWidth / 2.0F, 0.0F, lvlHeight / 2.0F), radius);

        transform.position = GetCameraPosition();
    }
#endif
}

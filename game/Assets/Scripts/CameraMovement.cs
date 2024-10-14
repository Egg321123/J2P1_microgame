using System;
using System.Globalization;
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
                float moveAmount = touch.deltaPosition.y / Screen.height * TAU;
                amount -= moveAmount;
                break;
            case 2:
                Touch t1 = Input.GetTouch(0);
                Touch t2 = Input.GetTouch(1);
                radius += Vector2.Distance(t1.deltaPosition, t2.deltaPosition) * 0.01F;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignCameraToScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Level level = FindFirstObjectByType<MonoLevel>().Level;

        if (level != null)
        {
            float width = level.width;
            transform.position = new(width / 2, transform.position.y, transform.position.z);
        }
    }
}

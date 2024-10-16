using UnityEngine;

public class FrameRateHandler : MonoBehaviour
{
    [SerializeField] private int targetFrameRate = 60;

    // start is called before the first frame
    void Start()
    {
        QualitySettings.vSyncCount = 0;                 // disable VSync, because android doesn't treat it nicely
        Application.targetFrameRate = targetFrameRate;  // set the target framerate to the target framerate (duh)
    }
}

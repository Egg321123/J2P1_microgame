using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateHandler : MonoBehaviour
{
    [SerializeField] private int targetFrameRate = 60;

    void Start()
    {
        QualitySettings.vSyncCount = 0; // Disable VSync
        Application.targetFrameRate = targetFrameRate;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private Waves waves;

    private void Start()
    {
        waves = GameManager.Instance.Waves;
    }

    public void Update()
        {
        if (slider == null || waves == null)
        {
            Debug.LogWarning("Slider or Waves is not assigned!");
            return;
        }

        if (waves.Wave >= 1 && waves.Wave <= 5)
        {
            slider.value = waves.Wave + 1;
        }
        else
        {
            Debug.LogWarning("Wave value is out of range!");
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private Waves waves;

    private void Start()
    {
        waves = GameManager.Instance.Waves;

        if (slider == null || waves == null)
        {
            Debug.LogWarning("Slider or Waves is not assigned!");
            return;
        }
        slider.minValue = 0;
        slider.maxValue = waves.MaxWaves;
        slider.value = waves.Wave + 1;

        //Adds the method to the event, so it's called when it's triggered
        waves.NewWave += OnNewWave;
    }

    public void OnNewWave()
    {
        slider.value = waves.Wave + 1;
    }
}

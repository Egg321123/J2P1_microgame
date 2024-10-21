using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private Save save;

    private void Start()
    {
        save = GameManager.Instance.Save;

        if (slider == null || save == null)
        {
            Debug.LogWarning("Slider or Save is not assigned!");
            return;
        }
        slider.minValue = 0;
        slider.maxValue = SaveData.MAX_HP;
        slider.value = save.data.hp;

        //Adds the method to the event, so it's called when it's triggered
        GameManager.Instance.Waves.HealthDecreased += OnHealthDecreased;
        GameManager.Instance.Waves.NewWave += OnHealthDecreased;
    }

    public void OnHealthDecreased()
    {
        slider.value = save.data.hp;
    }
}

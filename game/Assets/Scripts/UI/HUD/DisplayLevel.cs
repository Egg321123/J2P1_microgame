using TMPro;
using UnityEngine;

public class DisplayLevel : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Waves.NewWave += UpdateShownLevel;
        UpdateShownLevel();
    }

    private void UpdateShownLevel()
    {
        text.text = $"Lvl {GameManager.Instance.Save.data.level + 1}";
    }
}

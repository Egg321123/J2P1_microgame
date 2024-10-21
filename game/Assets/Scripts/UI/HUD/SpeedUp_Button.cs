using UnityEngine;
using UnityEngine.UI;

public class SpeedUp_Button : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite speedUpIcon;
    [SerializeField] private Sprite normalSpeedIcon;


    private int normalTime = 1;
    private int speedUpTime = 3;
    private int clickTimes;

    private void Start()
    {
        GameManager.Instance.Waves.NewWave += CheckSpeed;
    }

    private void CheckSpeed()
    {
        if (Time.timeScale != speedUpTime)
        {
            buttonImage.sprite = normalSpeedIcon;
            clickTimes = 0;
        }
    }

    public void ToggleSpeed()
    {
        clickTimes++;

        if (clickTimes == 1) {
            Time.timeScale = speedUpTime;
            buttonImage.sprite = speedUpIcon;
        } else {
            Time.timeScale = normalTime;
            buttonImage.sprite = normalSpeedIcon;
            clickTimes = 0;
        }
    }


}

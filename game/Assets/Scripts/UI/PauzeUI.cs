using UnityEngine;

public class PauzeUI : MonoBehaviour
{
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject pauseUI;
    bool gameIsFreezing = false;
    public void FreezeSwitch()
    {
        if (!gameIsFreezing) Pause(0);
        else Pause(1);

        gameIsFreezing = !gameIsFreezing;
    }
    void Pause(int time)
    {
        Time.timeScale = time;
        gameUI.SetActive(gameIsFreezing);
        pauseUI.SetActive(!gameIsFreezing);
    }
}

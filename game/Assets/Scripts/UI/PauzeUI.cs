using UnityEngine;

public class PauzeUI : MonoBehaviour
{
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject pauseUI;
    private float prevTime = 1.0F;
    public void Pause(bool goalIsActive)//this method activate and deactivate the pause menue based on the parameter
    {
        if (goalIsActive)
        {
            prevTime = Time.timeScale;
            Time.timeScale = 0;//if the goal is to activate the pause menu than freeze the game, otherwise the game will play as normal
        }
        else Time.timeScale = prevTime;

        gameUI.SetActive(!goalIsActive);
        pauseUI.SetActive(goalIsActive);
    }
    private void OnApplicationFocus(bool focus)//if the aplication loses the foces it will automaticly acitavte the pause menu
    {
        if (!focus) Pause(true);
    }
}

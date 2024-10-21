using UnityEngine;

public class PauzeUI : MonoBehaviour
{
    private float originalTime = 0;

    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject pauseUI;
    public void Pause(bool goalIsActive)//this method activate and deactivate the pause menue based on the parameter
    {
        //if the goal is to activate the pause menu than freeze the game, otherwise the game will play as normal
        if (goalIsActive) {
            originalTime = Time.timeScale;
            Time.timeScale = 0;
        } else Time.timeScale = originalTime;

        gameUI.SetActive(!goalIsActive);
        pauseUI.SetActive(goalIsActive);
    }
    private void OnApplicationFocus(bool focus)//if the aplication loses the foces it will automaticly acitavte the pause menu
    {
        if (!focus) Pause(true);
    }
}

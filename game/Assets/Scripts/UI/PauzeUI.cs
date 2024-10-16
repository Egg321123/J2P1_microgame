using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauzeUI : MonoBehaviour
{
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject pauseUI;
    public void Pause(bool goalIsActive)//this method activate and deactivate the pause menue based on the parameter
    {
        if (goalIsActive) Time.timeScale = 0;//if the goal is to activate the pause menu than freeze the game, otherwise the game will play as normal
        else Time.timeScale = 1;

        gameUI.SetActive(!goalIsActive);
        pauseUI.SetActive(goalIsActive);
    }
    private void OnApplicationFocus(bool focus)//if the aplication loses the foces it will automaticly acitavte the pause menu
    {
        if (!focus) Pause(true);
    }
}

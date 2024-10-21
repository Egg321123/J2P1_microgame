using UnityEngine;

public class PauzeUI : MonoBehaviour
{
    private float originalTime = 0;

    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject pauseUI;

    public void Pause(bool goalIsActive) // activates and deactivates the pause menu based on the parameter
    {
        //if the goal is to activate the pause menu than freeze the game, otherwise the game will play as normal
        if (goalIsActive) {
            originalTime = Time.timeScale;
            Time.timeScale = 0;
        } else Time.timeScale = originalTime;

        gameUI.SetActive(!goalIsActive);
        pauseUI.SetActive(goalIsActive);
    }

    // this caused issues with the speed up button and other timings when another UI was visible, so disabled
    // furthermore, it actually was quite annoying.
    /*
    private void OnApplicationFocus(bool focus) //if the application loses the focus it will automatically activate the pause menu
    {
        if (!focus) Pause(true);
    }
    */
}

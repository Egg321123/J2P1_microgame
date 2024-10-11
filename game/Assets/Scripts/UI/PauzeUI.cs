using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauzeUI : MonoBehaviour
{
    [SerializeField] GameObject Shop;
    [SerializeField] GameObject Pauze;
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
        Shop.SetActive(gameIsFreezing);
        Pauze.SetActive(!gameIsFreezing);

    }
}

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
        if (!gameIsFreezing) name(0);
        else name(1);
            gameIsFreezing = !gameIsFreezing;
    }
    void name(int time)
    {
        Time.timeScale = time;
        Shop.SetActive(gameIsFreezing);
        Pauze.SetActive(!gameIsFreezing);

    }
}

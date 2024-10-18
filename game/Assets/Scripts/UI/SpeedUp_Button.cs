using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class SpeedUp_Button : MonoBehaviour
{
    private int normalTime = 1;
    private int speedUpTime = 3;
    private int clickTimes;
    public void OnHold()
    {
        clickTimes++;
        if (clickTimes == 1)
        {
            Time.timeScale = speedUpTime;
        }
        if (clickTimes == 2)
        {
            Time.timeScale = normalTime;
        }
        if (clickTimes >= 2)
        {
            clickTimes = 0;
        }
    }
}

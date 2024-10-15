using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchItems : MonoBehaviour
{
    [SerializeField] GameObject gO2;
    public void Switch()
    {
        gO2.SetActive(true);
        gameObject.SetActive(false);
    }
}

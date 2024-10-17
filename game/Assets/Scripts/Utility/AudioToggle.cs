using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioToggle : MonoBehaviour
{
    [SerializeField] Toggle FXToggle;
    [SerializeField] Toggle MusicToggle;

    [SerializeField] AudioMixer audioMixer;

    private void Start()
    {
        //get the toggles from playerprefs
        FXToggle.isOn = PlayerPrefs.GetInt("FXToggle", 1) == 1;
        MusicToggle.isOn = PlayerPrefs.GetInt("MusicToggle", 1) == 1;

        ToggleFX();
        ToggleMusic();
    }

    public void ToggleFX()
    {
        bool isOn = FXToggle.isOn;
        float volume = isOn ? 0 : -80;

        audioMixer.SetFloat("FXVol", volume);
        PlayerPrefs.SetInt("FXToggle", Convert.ToInt32(isOn));
    }

    public void ToggleMusic()
    {
        bool isOn = MusicToggle.isOn;
        float volume = isOn ? 0 : -80;

        audioMixer.SetFloat("MusicVol", volume);
        PlayerPrefs.SetInt("MusicToggle", Convert.ToInt32(isOn));
    }
}

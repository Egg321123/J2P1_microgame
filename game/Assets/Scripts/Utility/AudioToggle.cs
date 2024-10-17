using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioToggle : MonoBehaviour
{
    [SerializeField] Toggle toggleFX;
    [SerializeField] Toggle toggleMusic;
    [SerializeField] Toggle toggleAmbience;

    [SerializeField] AudioMixer audioMixer;

    private void Start()
    {
        //get the toggles from playerprefs
        toggleFX.isOn = PlayerPrefs.GetInt("toggleFX", 1) == 1;
        toggleMusic.isOn = PlayerPrefs.GetInt("toggleMusic", 1) == 1;
        toggleMusic.isOn = PlayerPrefs.GetInt("toggleAmbiences", 1) == 1;

        ToggleFX();
        ToggleMusic();
        ToggleAmbience();
    }

    public void ToggleFX()
    {
        bool isOn = toggleFX.isOn;
        float volume = isOn ? 0 : -80;

        audioMixer.SetFloat("FXVol", volume);
        PlayerPrefs.SetInt("toggleFX", Convert.ToInt32(isOn));
    }

    public void ToggleMusic()
    {
        bool isOn = toggleMusic.isOn;
        float volume = isOn ? 0 : -80;

        audioMixer.SetFloat("MusicVol", volume);
        PlayerPrefs.SetInt("toggleMusic", Convert.ToInt32(isOn));
    }

    public void ToggleAmbience()
    {
        bool isOn = toggleAmbience.isOn;
        float volume = isOn ? 0 : -80;

        audioMixer.SetFloat("AmbienceVol", volume);
        PlayerPrefs.SetInt("toggleAmbiences", Convert.ToInt32(isOn));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioToggle : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    bool isAudioOn = true;

    public void ToggleAudio()
    {
        float volume = isAudioOn ? -80 : 0;
        audioMixer.SetFloat("MainVolume", volume);
        isAudioOn = !isAudioOn;
    }
}

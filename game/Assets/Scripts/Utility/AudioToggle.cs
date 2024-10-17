using UnityEngine;
using UnityEngine.Audio;

public class AudioToggle : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    bool isAudioOn = true;

    public void ToggleFX()
    {
        float volume = isAudioOn ? -80 : 0;
        audioMixer.SetFloat("FXVol", volume);
        isAudioOn = !isAudioOn;
    }

    public void ToggleMusic()
    {
        float volume = isAudioOn ? -80 : 0;
        audioMixer.SetFloat("MusicVol", volume);
        isAudioOn = !isAudioOn;
    }
}

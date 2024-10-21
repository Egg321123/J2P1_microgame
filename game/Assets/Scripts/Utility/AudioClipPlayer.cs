using System.Collections;
using UnityEngine;

public class AudioClipPlayer : MonoBehaviour
{
    [SerializeField] AudioSource source;

    public void Initialize(AudioClip clip, bool realTime = false, int priority = 128) => Initialize(clip, priority, realTime);
    public void Initialize(AudioClip clip, int priority = 128, bool realTime = false)
    {
        StartCoroutine(DestroyAfterPlay(clip, realTime, priority));
    }

    IEnumerator DestroyAfterPlay(AudioClip clip, bool realTime, int priority)
    {
        float clipLength = clip.length;

        source.clip = clip;
        source.priority = priority;

        if (!realTime && Time.timeScale != 1) source.pitch = Time.timeScale / 2;
        else source.pitch = Random.Range(0.8f, 1.2f);

        source.Play();

        if (realTime) yield return new WaitForSecondsRealtime(clipLength);
        else yield return new WaitForSeconds(clipLength);

        Destroy(gameObject);

        yield return null;
    }
}

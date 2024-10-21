using System.Collections;
using UnityEngine;

public class AudioClipPlayer : MonoBehaviour
{
    [SerializeField] AudioSource source;

    public void Initialize(AudioClip clip)
    {
        StartCoroutine(DestroyAfterPlay(clip));
    }

    IEnumerator DestroyAfterPlay(AudioClip clip)
    {
        float clipLength = clip.length;

        source.clip = clip;
        source.pitch = (Random.Range(0.8f, 1.2f));
        source.Play();

        yield return new WaitForSecondsRealtime(clipLength);

        Destroy(gameObject);

        yield return null;
    }
}

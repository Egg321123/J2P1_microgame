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
        source.pitch = (Random.Range(0.4f, .9f));
        source.Play();

        yield return new WaitForSeconds(clipLength);

        Destroy(gameObject);

        yield return null;
    }
}

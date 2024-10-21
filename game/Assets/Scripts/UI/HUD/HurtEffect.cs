using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HurtEffect : MonoBehaviour
{
    [SerializeField] Image img;
    Coroutine coroutine;

    Color startColor = new(1, 0, 0, 0);
    Color endColor = new(1, 0, 0, 0.7f);

    private void Start() => GameManager.Instance.Waves.HealthDecreased += TriggerHurtEffect;

    public void TriggerHurtEffect() => coroutine ??= StartCoroutine(Hurt());

    private IEnumerator Hurt()
    {
        float duration = 0.1f;
        float time = 0;

        while (time < duration)
        {
            img.color = Color.Lerp(startColor, endColor, time / duration);

            time += Time.unscaledDeltaTime;

            yield return null;
        }

        img.color = endColor;

        duration = 0.2f;
        time = 0;

        while (time < duration)
        {
            img.color = Color.Lerp(endColor, startColor, time / duration);

            time += Time.unscaledDeltaTime;

            yield return null;
        }

        img.color = startColor;

        //empty coroutine
        coroutine = null;
        yield return null;  
    }


}

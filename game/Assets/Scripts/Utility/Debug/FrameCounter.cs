using UnityEngine;
using TMPro;
using System.Linq;

public class FrameCounter : MonoBehaviour
{
#if DEBUG
    private TMP_Text fpsCounter;

    private int lastFrameIndex;
    private float[] frameDeltaTimeArray;

    private void Awake()
    {
        fpsCounter = GetComponent<TMP_Text>();
        frameDeltaTimeArray = new float[50];
    }

    // Update is called once per frame
    private void Update()
    {
        frameDeltaTimeArray[lastFrameIndex] = Time.unscaledDeltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

        fpsCounter.text = Mathf.RoundToInt(CalculateFPS()).ToString();
    }

    // get the calculated FPS
    private float CalculateFPS()
    {
        float total = frameDeltaTimeArray.Sum();
        return frameDeltaTimeArray.Length / total;
    }
#else
// if this is a release, just remove the fps count on awake
    private void Awake()
    {
        Destroy(GetComponent<TMP_Text>().gameObject);
    }
#endif
}

using UnityEngine;
using TMPro;
using System;

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
    void Update()
    {
        frameDeltaTimeArray[lastFrameIndex] = Time.unscaledDeltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

        fpsCounter.text = Mathf.RoundToInt(CalculateFPS()).ToString();
    }

    private float CalculateFPS()
    {
        float total = 0f;
        foreach (float deltaTime in frameDeltaTimeArray)
        {
            total += deltaTime;
        }

        return frameDeltaTimeArray.Length / total;
    }
#else
    private void Awake() {
        Destroy(GetComponent<TMP_Text>().GameObject);
    };
#endif
}

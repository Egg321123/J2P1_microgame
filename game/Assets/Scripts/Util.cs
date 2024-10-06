using System;
using UnityEngine;

public static class Util
{
    /// <returns>how long it took to execute <paramref name="f"/> in seconds</returns>
    public static float GetTime(Action f) {
        float time = Time.time;
        f();
        return Time.time - time;
    }
}

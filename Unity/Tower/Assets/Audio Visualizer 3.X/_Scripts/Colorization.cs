using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorization : MonoBehaviour
{
    public static void Colorize(int tokenBand, bool Red, bool Green, bool Blue, bool Transition, GameObject Object)
    {
        int R = 0;
        int G = 0;
        int B = 0;
        int T = 0;

        if (Red) R = 1;
        if (Green) G = 1;
        if (Blue) B = 1;
        if (Transition) T = 1;

        float V = Audio_3_1._currentBandAudio[tokenBand];

        Color color = new Color(
            R * (V + (T * ((B - 0.35f) * G) + (1 - T) * ((G - 0.35f) * B))),
            G * (V + (T * ((R - 0.35f) * B) + (1 - T) * ((B - 0.35f) * R))),
            B * (V + (T * ((G - 0.35f) * R) + (1 - T) * ((R - 0.35f) * G)))) *
            (R + G + B == 0 || R + G + B == 3 ? 0 : 1) +

            V * Color.white *
            (R + G + B == 0 ? 1 : 0);

        Object.GetComponent<Renderer>().material.SetColor("_Color", color);
        Object.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
    }
}

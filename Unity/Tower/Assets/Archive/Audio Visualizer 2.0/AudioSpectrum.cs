using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{
    public int sample_size_audio;
    public int sample_size_spectrum;

    private float[] audioSpectrum;
    public static float spectrumValue { get; private set; }
    // Start is called before the first frame update
    private void Start()
    {
        audioSpectrum = new float[sample_size_audio];
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.GetSpectrumData(audioSpectrum, 0, FFTWindow.BlackmanHarris);
        if (audioSpectrum != null && audioSpectrum.Length > 0) {
            spectrumValue = audioSpectrum[0] * sample_size_spectrum;
        }
    }
}

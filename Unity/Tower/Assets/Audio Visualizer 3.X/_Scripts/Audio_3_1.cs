using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Audio_3_1 : MonoBehaviour
{
    public float _sensibility;
    public float _minThreshold;

    public static int _numberOfSamples = 8192;
    public static int _numberOfFrequencyBands = 8;

    public static bool _beatDetected;

    public static float[] _sampleAudio;
    public static float[] _sampleSpectrum;

    public static float[] _currentBandFreq;
    private float[] _lastBandFreq;
    private float[] _highestBandFreq;

    public static float[] _currentBandAudio;
    private float[] _lastBandAudio;
    private float[] _highestBandAudio;

    public static float _currentAmplitude;
    private float _lastAmplitude;
    private float _highestAmplitude;

    public static float _currentVariance;
    private float _lastVariance;
    private float _highestVariance;

    AudioSource _audioSource;

    private int[] _bandSize = { 14, 44, 52, 185, 631, 929, 1858, 4479 };

    private float _lastAverageFreq;
    private float _currentAverageFreq;

    private float _lastAverageAudio;
    private float _currentAverageAudio;

    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        _sampleAudio = new float[_numberOfSamples];
        _sampleSpectrum = new float[_numberOfSamples];
        _currentBandFreq = new float[_numberOfFrequencyBands];
        _currentBandAudio = new float[_numberOfFrequencyBands];
        _lastBandFreq = new float[_numberOfFrequencyBands];
        _highestBandFreq = new float[_numberOfFrequencyBands];

        Light light = GameObject.Find("Dir Light (1)").GetComponent<Light>();
        light.transform.rotation = Quaternion.Euler(270, 0, 0);
        //GameObject.Find("Ground").SetActive(false); WTF

        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(AudioSettings.outputSampleRate);
        GetAudioSource();
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        CreateAudioBands();
        GetAverage();
        GetAmplitude();
        GetVariance();
        DetectBeat();
    }

    void GetAudioSource()
    {
        _audioSource.GetOutputData(_sampleAudio, 0);
    }

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_sampleSpectrum, 0, FFTWindow.BlackmanHarris);
        _sampleSpectrum[0] = _sampleSpectrum[1] * (Random.Range(0, 100) / 100);
    }

    void MakeFrequencyBands()
    {
        int count = 0;
        int sizeCount = 0;
        for (int i = 0; i < _numberOfFrequencyBands; i++)
        {
            float total = 0;
            int sampleCount = _bandSize[sizeCount];
            for (int j = 0; j < sampleCount; j++)
            {
                total += _sampleSpectrum[count] * (count + 1);
                count++;
            }
            sizeCount++;
            _currentBandFreq[i] = total / count;
        }
    }

    void CreateAudioBands()
    {
        for (int i = 0; i < _numberOfFrequencyBands; i++)
        {
            if (_currentBandFreq[i] > _highestBandFreq[i] || _highestBandFreq[i] == 0)
            {
                _highestBandFreq[i] = _currentBandFreq[i];
            }
            if (_highestBandFreq[i] != 0)
            {
                _currentBandAudio[i] = (_currentBandFreq[i] / _highestBandFreq[i]);
            }
        }
    }

    void GetAverage()
    {
        _lastAverageAudio = _currentAverageAudio;
        _lastAverageFreq = _currentAverageFreq;

        _currentAverageAudio = 0;
        _currentAverageFreq = 0;

        for (int i = 0; i < _numberOfFrequencyBands; i++)
        {
            _currentAverageAudio += _currentBandAudio[i];
            _currentAverageFreq += _currentBandFreq[i];
        }

        _currentAverageAudio /= _numberOfFrequencyBands;
        _currentAverageFreq /= _numberOfFrequencyBands;
    }

    void GetAmplitude()
    {
        _lastAmplitude = _currentAmplitude;
        _currentAmplitude = 0;

        for (int i = 0; i < _numberOfFrequencyBands; i++)
        {
            _currentAmplitude += _currentBandAudio[i];
        }

        if (_currentAmplitude > _highestAmplitude || _currentAmplitude == 0)
        {
            _highestAmplitude = _currentAmplitude;
        }
        if (_highestAmplitude != 0)
        {
            _currentAmplitude /= _highestAmplitude;
        }
    }

    void GetVariance()
    {
        _lastVariance = _currentVariance;
        _currentVariance = 0;

        for (int i = 0; i < _numberOfFrequencyBands; i++)
        {
            _currentVariance += Mathf.Pow(_currentBandAudio[i] - _currentAverageAudio, 2);
        }
        _currentVariance /= _numberOfFrequencyBands;

        if (_currentVariance > _highestVariance || _currentVariance == 0)
        {
            _highestVariance = _currentVariance;
        }
        if (_highestVariance != 0)
        {
            _currentVariance /= _highestVariance;
        }
    }

    void DetectBeat()
    {
        if (_audioSource.isPlaying)
        {
            /*
            
            // ***** First Try ***** ----- AVERAGE
            if (_currentAverageValue > _lastAverageValue * _sensibility)
            {
                Debug.Log("Beat Detected");
            }

            */
            /*
            
            // ***** Second Try ***** ----- BAND > LAST && THRESHOLD
            for (int i = 0; i < _numberOfFrequencyBands; i++)
            {
                if (_bandFreq[i] > _lastFreqBand[i] * _sensibility && _bandFreq[i] > _minThreshold)
                {
                    Debug.Log("Beat Detected");
                }
                _lastFreqBand[i] = _freqBand[i];
            }

            */
            /*
            
            // ***** Third Try ***** ----- AVERAGE + BAND > LAST && THRESHOLD
            if (_currentAverageFreq > _lastAverageFreq)
            {
                for (int i = 0; i < _numberOfFrequencyBands; i++)
                {
                    if (_bandFreq[i] > _lastFreqBand[i] * _sensibility && _bandFreq[i] > _minThreshold)
                    {
                        Debug.Log("BEAT DETECTED");
                    }
                    _lastFreqBand[i] = _bandFreq[i];
                }
            }

            */

            // ***** Fourth Try ***** ----- AMPLITUDE
            if (_currentAmplitude > _sensibility * _lastAmplitude && timer > 0.25)
            {
                Debug.Log("BEAT DETECTED");
                _beatDetected = true;
                timer = 0;
            }
            timer += Time.deltaTime;

            /*

            // ***** Fifth Try ***** ----- POSITIVE VARIANCE
            if (_currentVariance > _sensibility * _lastVariance)
            {

                Debug.Log("BEAT DETECTED");
            }

            */
        }
    }
}

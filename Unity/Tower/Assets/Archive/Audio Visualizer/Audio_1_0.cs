using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof (AudioSource))]
public class Audio_1_0 : MonoBehaviour {
	public int nbElements;
	public int nbBarres;

	private AudioSource audioSource;

	public static float amplitude, amplitudeBuffer;
	public float audioProfile;
	private float amplitudeHighest;

	public static float[] samples;
	public static float[] freqBand;
	public static float[] bandBuffer;
	public static float[] audioBand;
	public static float[] audioBandBuffer;

	private float[] bufferDecrease;
	private float[] freqBandHighest;
	
	void Start () {
		samples = new float[nbElements];
		freqBand = new float[nbBarres];
		bandBuffer = new float[nbBarres]; 
		bufferDecrease = new float[nbBarres];
		freqBandHighest = new float[nbBarres];
		audioBand = new float[nbBarres];
		audioBandBuffer = new float[nbBarres];
		audioSource = GetComponent<AudioSource> ();
		AudioProfile (audioProfile);
	}

	void Update () {
		GetSpectrumAudioSource ();
		MakeFrequencyBands ();
		BandBuffer ();
		CreateAudioBands ();
		GetAmplitude ();
	}

	void GetSpectrumAudioSource(){
		audioSource.GetSpectrumData (samples, 0, FFTWindow.BlackmanHarris);
	}

	void MakeFrequencyBands(){
		int count = 0;
		freqBand [0] += samples [0] * 10;
		for (int i = 0; i < nbBarres; i++) {
			float average = 0;
			int sampleCount = (int)Mathf.Pow (2, i);
			for (int j = 0; j < sampleCount; j++) {
				average += samples [count] * (i + 1);
				count++;
			}
			average = average * 25 / sampleCount;
			freqBand [i] = average;
		}
	}

	void BandBuffer(){
		for (int g = 0; g < nbBarres; g++){
			if (freqBand [g] > bandBuffer [g]) {
				bandBuffer [g] = freqBand [g];
				bufferDecrease [g] = 0.001f;
			}
			if (freqBand [g] < bandBuffer [g]) {
				bandBuffer [g] -= bufferDecrease [g];
				bufferDecrease [g] *= 1.25f;
			}
		}
	}

	void CreateAudioBands(){
		for (int i = 0; i < nbBarres; i++) {
			if (freqBand [i] > freqBandHighest [i]) {
				freqBandHighest [i] = freqBand [i];
			}
			audioBand [i] = (freqBand [i] / freqBandHighest [i]);
			audioBandBuffer [i] = (bandBuffer [i] / freqBandHighest [i]);
		}
	}

	void GetAmplitude(){
		float currentAmplitude = 0;
		float currentAmplitudeBuffer = 0;
		for (int i = 0; i < nbBarres; i++) {
			currentAmplitude += audioBand [i];
			currentAmplitudeBuffer += audioBandBuffer [i];
		}
		if (currentAmplitude > amplitudeHighest) {
			amplitudeHighest = currentAmplitude;
		}
		amplitude = currentAmplitude / amplitudeHighest;
		amplitudeBuffer = currentAmplitudeBuffer / amplitudeHighest;
	}

	void AudioProfile(float audioProfile){
		for (int i = 0; i < nbBarres; i++) {
			freqBandHighest [i] = audioProfile;
		}	
	}
}

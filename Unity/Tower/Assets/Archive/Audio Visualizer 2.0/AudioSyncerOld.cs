using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncer : MonoBehaviour
{
    protected bool isBeat;

    private float timer;
    private float audioValue;
    private float previousAudioValue;

    public float bias;
    public float timeStep;
    public float timeToBeat;
    public float restSmoothSpeed;

    // Update is called once per frame
    private void Update()
    {
        OnUpdate();
    }

    public virtual void OnUpdate()
    {
        previousAudioValue = audioValue;
        audioValue = Audio_2_0.spectrumValue;
        /*
        if (previousAudioValue > bias && audioValue <= bias)
        {
            if (timer > timeStep)
                OnBeat();
        }
        */
        if (previousAudioValue <= bias && audioValue > bias)
        {
            if (timer > timeStep)
                OnBeat();
        }
        timer += Time.deltaTime;
    }

    public virtual void OnBeat()
    {
        Debug.Log("Beat detected !");
        //spawn();
        timer = 0;
        isBeat = true;
    }
}

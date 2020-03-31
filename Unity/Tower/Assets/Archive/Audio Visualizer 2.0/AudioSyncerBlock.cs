using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncerBlock : GridBlockGenerator{
    protected bool isBeat;

    private new float timer;    //NEW AJOUTE ICI A PEUT ETRE ENLEVER
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
            {
                OnBeat();
            }
        }
        timer += Time.deltaTime;
    }

    public virtual void OnBeat()
    {
        spawnBlock();
        Debug.Log("Beat detected !");
        timer = 0;
        isBeat = true;
    }
}

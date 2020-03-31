using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBeatGenerator : Generator
{
    public bool actif;

    public override void generate()
    {
        if (timer > interval && Audio_3_1._beatDetected && actif)
        {
            Audio_3_1._beatDetected = false;
            OnBeat();
        }
        timer += Time.deltaTime;
    }

    public virtual void OnBeat()
    {
        gridManager.spawnBlocks();
        timer = 0;
    }

}

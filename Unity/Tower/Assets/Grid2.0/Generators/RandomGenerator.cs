using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerator : Generator{

    public override void generate() {
        timer += Time.deltaTime;
        if (timer > interval) {
            gridManager.spawnBlocks();
            timer = 0;
        }  
    }
}

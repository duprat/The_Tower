using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGeneratorWSpawnables : Generator{

    [Range(2, 10)]
    public int itemProbability = 10;

    public override void generate() {
        timer += Time.deltaTime;
        if (timer > interval) {
            if (Random.Range(0, itemProbability) > 0 || spawnableItems.Count == 0)gridManager.spawnBlocks();
            else {
                gridManager.spawnSpawnable(spawnableItems[Random.Range(0, spawnableItems.Count)]);
            }
            timer = 0;
        }
    }
}

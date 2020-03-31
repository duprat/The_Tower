using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerBlockGenerator : Generator {

    public override void generate() {
        timer += Time.deltaTime;
        if (timer > interval) {
            gridManager.spawnBlocks(new Vector2Int(getPlayerBlock().x, getPlayerBlock().y), 1);
            timer = 0;
        }
    }
}

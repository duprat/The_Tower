using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerSideGenerator : Generator {

    public bool actif;
    public override void generate() {
        timer += Time.deltaTime;
        if (timer > interval && actif) {
            switch (getPlayerSide()) {
                case Orientation.FORWARD:
                    gridManager.spawnBlocks(new Vector2Int(Random.Range(0, gridManager.getGridSize()), gridManager.getGridSize() - 1), 1);
                    break;
                case Orientation.RIGHT:
                    gridManager.spawnBlocks(new Vector2Int(gridManager.getGridSize() - 1, Random.Range(0, gridManager.getGridSize())), 1);
                    break;
                case Orientation.BACK:
                    gridManager.spawnBlocks(new Vector2Int(Random.Range(0, gridManager.getGridSize()), 0), 1);
                    break;
                case Orientation.LEFT:
                    gridManager.spawnBlocks(new Vector2Int(0, Random.Range(0, gridManager.getGridSize())), 1);
                    break;
            }
            timer = 0;
        }
    }

}

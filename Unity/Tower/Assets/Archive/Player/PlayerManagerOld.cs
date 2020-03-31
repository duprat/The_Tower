﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerOld : MonoBehaviour{
    public bool hasGridAssigned = false;
    public GameObject gridObject;
    GridPlayerManager grid;

    private void Start() {
        grid = gridObject.GetComponent<GridPlayerManager>();
    }

    private void Update() {
        if (!hasGridAssigned && grid != null) {
            grid.setPlayer(GetComponent<PlayerController>());
            hasGridAssigned = true;
        }
    }

    public void playerDies(){
        grid.playerDied();
    }

}

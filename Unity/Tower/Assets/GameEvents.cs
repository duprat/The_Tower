using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour{

    public static GameEvents current;

    private void Awake() {
        current = this;
    }

    public event Action<int> onBlockLanding;

    public void BlockLanding(int id) {
        if (onBlockLanding != null) {
            onBlockLanding(id);
        }
    }

    public event Action onPlayerDeath;

    public void PlayerDeath() {
        if (onPlayerDeath != null) {
            onPlayerDeath();
        }
    }

    public event Action onChangeCamera;

    public void ChangeCamera() {
        if (onChangeCamera != null) {
            onChangeCamera();
        }
    }
}

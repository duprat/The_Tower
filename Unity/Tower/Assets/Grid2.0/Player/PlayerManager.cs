using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour{

    protected bool hasGridAssigned = false;
    protected PlayerController controller;
    GridManager grid;

    private void Start() {
        grid = GameObject.FindWithTag("Grid").GetComponent<GridManager>();
        controller = GetComponent<PlayerController>();
    }

    private void Update() {
        if (!hasGridAssigned && grid != null)
        {
            grid.setPlayer(this);
            hasGridAssigned = true;
        }
    }

    public void setAngle(float physicsAngle, float graphicsAngle) {
        controller.setAngle(physicsAngle, graphicsAngle);
    }

    public void moveTo (Vector3 position) {
        transform.position = position;
        controller.hasMoved = true;
    }

    public Vector3 getPosition() {
        return controller.transform.position;
    }

    public void kill(GameObject killer) {
        GameEvents.current.PlayerDeath();
        grid.removePlayer();
        Destroy(gameObject);

    }

    public bool hasMoved() { return controller.hasMoved; }
    public bool isGrounded() { return controller.isGrounded; }
}

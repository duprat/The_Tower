using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlayerManager : MonoBehaviour{
    protected bool hasAssignedPlayer = false;
    protected TowerGrid grid;
    protected PlayerController player;
    protected Vector3 playerCurrentSide;
    public float sideChangeBuffer = 0.05f;

    private void Start() {
        grid = GetComponent<TowerGrid>();
    }

    private void Update() {
        if (hasAssignedPlayer && player.hasMoved)
            playerSideChangeCheck();
    }

    public bool setPlayer(PlayerController pc) {
        if (hasAssignedPlayer) return false;
        player = pc;
        hasAssignedPlayer = true;
        placePlayerRandom();
        return true;
    }

    public PlayerController removePlayer() {
        PlayerController pc = player;
        player = null;
        hasAssignedPlayer = false;
        return pc;
    }

    void placePlayerRandom() {
        float playerHalfSize = player.GetComponent<Collider>().bounds.size.y + 1;
        player.transform.position = grid.getSortedColumns()[Random.Range(0, grid.getSortedColumns().Count)].transform.position + Vector3.up * playerHalfSize;
        setPlayerCurrentSide();
    }

    void setPlayerCurrentSide() {
        if (player.transform.position.z >= grid.center.y + (grid.centeredDimensions.y / 2.0f)) playerCurrentSide = Vector3.forward;
        else if (player.transform.position.z <= grid.center.y - (grid.centeredDimensions.y / 2.0f)) playerCurrentSide = Vector3.back;
        else if (player.transform.position.x >= grid.center.x + (grid.centeredDimensions.x / 2.0f)) playerCurrentSide = Vector3.right;
        else if (player.transform.position.x <= grid.center.x + (grid.centeredDimensions.x / 2.0f)) playerCurrentSide = Vector3.left;
        setPlayerFacingAngle();
    }

    void setPlayerFacingAngle() {
        if (playerCurrentSide == Vector3.forward) player.setAngle(180);
        else if (playerCurrentSide == Vector3.back) player.setAngle(0);
        else if (playerCurrentSide == Vector3.right) player.setAngle(-90);
        else if (playerCurrentSide == Vector3.left) player.setAngle(90);
    }

    void playerSideChangeCheck() {
        int side = 0;
        if (playerCurrentSide.x != 0) {
            if (player.transform.position.z > grid.center.y + (grid.centeredDimensions.y / 2) + sideChangeBuffer) { side = 1; } else if (player.transform.position.z < grid.center.y - (grid.centeredDimensions.y / 2) - sideChangeBuffer) { side = -1; }
            if (side == 1) {
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, grid.center.y + (grid.centeredDimensions.y / 2));
                playerCurrentSide = Vector3.forward;
                setPlayerFacingAngle();
                Debug.Log("From x to forward");
            } else if (side < 0) {
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, grid.center.y - (grid.centeredDimensions.y / 2));
                playerCurrentSide = Vector3.back;
                setPlayerFacingAngle();
                Debug.Log("From x to back");
            }
        } else if (playerCurrentSide.z != 0) {
            if (player.transform.position.x > grid.center.x + (grid.centeredDimensions.x / 2) + sideChangeBuffer) { side = 1; } else if (player.transform.position.x < grid.center.x - (grid.centeredDimensions.x / 2) - sideChangeBuffer) { side = -1; }
            if (side > 0) {
                player.transform.position = new Vector3(grid.center.x + (grid.centeredDimensions.x / 2), player.transform.position.y, player.transform.position.z);
                playerCurrentSide = Vector3.right;
                setPlayerFacingAngle();
                Debug.Log("From z to right");
            } else if (side < 0) {
                player.transform.position = new Vector3(grid.center.x - (grid.centeredDimensions.x / 2), player.transform.position.y, player.transform.position.z);
                playerCurrentSide = Vector3.left;
                setPlayerFacingAngle();
                Debug.Log("From z to left");
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraMangager : MonoBehaviour{
    protected GridManager gridManager;
    protected GameObject cameraHolder;

    // Start is called before the first frame update
    void Awake() {
        gridManager = GetComponent<GridManager>();
        cameraHolder = transform.Find("CameraHolder").gameObject;
    }

    public abstract void updateCameraPosition();
    public abstract void setInitialValues();

    protected Orientation getPlayerSide() {
        return gridManager.getPlayerCurrentSide();
    }

    protected Vector3Int getPlayerBlock() { // 0;0 -> FORWARD-RIGHT corner
        return gridManager.getPlayerCurrentBlock();
    }

    protected Transform getPlayerTransform() {
        return gridManager.getPlayer().transform;
    }
}

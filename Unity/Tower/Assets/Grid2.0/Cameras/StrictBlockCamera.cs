using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrictBlockCamera : InGameCamera {
    [Header("Camera Control")]
    public float cameraUpOffset = 0;
    public float cameraBackOffset = 4;
    [Header("Look-at Control")]
    public float LookAtUpOffset = 0;

    public override void setInitialValues() {
        cameraHolder.transform.position = Vector3.zero;
        cameraHolder.transform.rotation = Quaternion.Euler(Vector3.zero);
        cameraHolder.transform.Find("Camera").transform.position = Vector3.forward * (gridManager.getGridSize() + cameraBackOffset) + Vector3.up * cameraUpOffset;
        cameraHolder.transform.Find("Camera").transform.LookAt(cameraHolder.transform.position + Vector3.up * LookAtUpOffset);
    }

    public override void updateCameraPosition() {
        cameraHolder.transform.position = new Vector3(Mathf.RoundToInt(gridManager.getPlayer().transform.position.x), Mathf.RoundToInt(gridManager.getPlayer().transform.position.y), Mathf.RoundToInt(gridManager.getPlayer().transform.position.z));
        cameraHolder.transform.rotation = Quaternion.Euler(0, (float)gridManager.getPlayerCurrentSide() + 180, 0);
    }
}

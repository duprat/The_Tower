using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrictSideCamera : InGameCamera {
    [Header("Holder Control")]
    public float cameraUpOffset = 4;
    public float cameraBackOffset = 8;
    [Header("Look-at Control")]
    public float LookAtUpOffset = 6;

    public override void setInitialValues() {
        cameraHolder.transform.position = Vector3.zero;
        cameraHolder.transform.rotation = Quaternion.Euler(Vector3.zero);
        cameraHolder.transform.Find("Camera").transform.position = Vector3.forward * (gridManager.getGridSize() + cameraBackOffset) + Vector3.up * cameraUpOffset;
        cameraHolder.transform.Find("Camera").transform.LookAt(cameraHolder.transform.position + Vector3.up * LookAtUpOffset);
    }

    public override void updateCameraPosition() {
        cameraHolder.transform.rotation = Quaternion.Euler(0, (float)gridManager.getPlayerCurrentSide() + 180, 0);
        cameraHolder.transform.position = new Vector3(cameraHolder.transform.position.x, gridManager.getPlayer().transform.position.y, cameraHolder.transform.position.z);
    }
}

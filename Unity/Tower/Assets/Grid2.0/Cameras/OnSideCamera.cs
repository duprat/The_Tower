using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSideCamera : InGameCamera{
    [Header("Holder Control")]
    public float cameraUpOffset = 4;
    public float cameraBackOffset = 4;
    [Header("Look-at Control")]
    public float LookAtUpOffset = 4;

    public override void setInitialValues() {
        cameraHolder.transform.position = Vector3.zero;
        cameraHolder.transform.rotation = Quaternion.Euler(Vector3.zero);
        cameraHolder.transform.Find("Camera").transform.position = Vector3.forward * (gridManager.getGridSize()+ cameraBackOffset) + Vector3.up * cameraUpOffset;
        cameraHolder.transform.Find("Camera").transform.LookAt(cameraHolder.transform.position + Vector3.up * LookAtUpOffset);
    }

    public override void updateCameraPosition() {
        cameraHolder.transform.rotation = gridManager.getPlayer().transform.rotation;
        cameraHolder.transform.position = new Vector3(cameraHolder.transform.position.x, gridManager.getPlayer().transform.position.y, cameraHolder.transform.position.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerCamera : InGameCamera{
    [Header("Camera Control")]
    public float cameraUpOffset = 4;
    public float cameraBackOffset = 4;
    [Header("Look-at Control")]
    public float LookAtUpOffset = 4;

    public override void setInitialValues() {
        cameraHolder.transform.position = Vector3.zero;
        cameraHolder.transform.rotation = Quaternion.Euler(Vector3.zero);
        cameraHolder.transform.Find("Camera").transform.position = Vector3.forward * (gridManager.getGridSize() + cameraBackOffset) + Vector3.up * cameraUpOffset;
        cameraHolder.transform.Find("Camera").transform.LookAt(cameraHolder.transform.position + Vector3.up * LookAtUpOffset);
    }

    public override void updateCameraPosition() {
        cameraHolder.transform.position = gridManager.getPlayer().transform.position;
        cameraHolder.transform.rotation = gridManager.getPlayer().transform.rotation;
    }
}

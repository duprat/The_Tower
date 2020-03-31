using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCamera : CameraMangager{
    public float panningSpeed = 1;

    public override void setInitialValues() {
        cameraHolder.transform.position = Vector3.zero;
        cameraHolder.transform.Find("Camera").transform.position = new Vector3(0, 13.85f, -22.96f);
        cameraHolder.transform.Find("Camera").transform.rotation = Quaternion.Euler(21.893f, 0, 0);
    }

    public override void updateCameraPosition() {
        cameraHolder.transform.RotateAround(cameraHolder.transform.position, Vector3.up, panningSpeed);
    }
}

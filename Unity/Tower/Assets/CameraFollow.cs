using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour{

    public GameObject toFollow;
    public float distance;

    private void Start() {
    }

    void Update(){
        if (toFollow != null) {
            transform.position = toFollow.transform.position + toFollow.transform.forward * distance;
            transform.LookAt(toFollow.transform);
        }
    }
}

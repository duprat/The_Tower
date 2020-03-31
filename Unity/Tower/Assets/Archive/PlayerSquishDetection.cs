using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSquishDetection : MonoBehaviour{

    public float collisionDistanceDetection;
    public LayerMask collisionMask;

    protected PlayerController pc;
    protected Rigidbody rb;
    protected PlayerManagerOld pm;
    protected RaycastHit[] hitBuffer = new RaycastHit[16];

    void Start(){
        pc = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerManagerOld>();
    }


    void Update(){
        bool death = false;
        hitBuffer = rb.SweepTestAll(transform.up, collisionDistanceDetection);
        for (int i = 0; i < hitBuffer.Length; i++) {
            if (collisionMask.value == 1 << hitBuffer[i].transform.gameObject.layer) {
                death = true;
            }
        }
        if (death) pm.playerDies();
    }
}

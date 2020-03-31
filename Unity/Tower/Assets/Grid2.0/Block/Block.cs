using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour{
    public static int blockCount = 0;

    public float fallSpeed = 8;
    [HideInInspector]
    public bool mobile = true;      //Block is allowed to fall
    [HideInInspector]
    public bool isFalling = true;   //Block is detecting ground or not
    [HideInInspector]
    public bool hasSettled = false; //Block has been !falling for settlingDelay time

    public LayerMask collisionLayers;

    protected bool exploding = false;
    protected int id = 0;
    protected float settlingDelay = 0.5f;
    protected float timer = 0.0f;
    protected float rayMargins = 0.01f;
    protected float raySupDistance = 0.5f;
    protected float rayDistance;
    protected Animation anim;

    private void Awake() {
        gameObject.SetActive(false);
    }

    private void Start() {
        id = blockCount++;
        anim = gameObject.GetComponentInChildren<Animation>();
        rayDistance = GetComponent<Collider>().bounds.extents.y;
    }

    private void FixedUpdate() {
        if (mobile) tryFall(); //If allowed try to fall

        if (!isFalling && !hasSettled) { //If not falling try to settle
            timer += Time.deltaTime;
            if (timer > settlingDelay) {
                hasSettled = true;
                //GameEvents.current.BlockLanding(id);
                timer = 0;
            }
        }
        if (hasSettled && isFalling) {   //If falling unsettle
            hasSettled = false;
            timer = 0;
        }

        if (exploding && !anim.isPlaying) {
            exploding = false;
            gameObject.SetActive(false);
        }
    }
    public void explode() {
        gameObject.layer = LayerMask.NameToLayer("Collisionless");
        anim.Play("Explode");
        exploding = true;
    }

    public void reset() {
        mobile = true;
        isFalling = true;   
        hasSettled = false; 
        timer = 0.0f;
        gameObject.layer = LayerMask.NameToLayer("Solid");
        gameObject.SetActive(true);
    }


    protected void tryFall() {
        Vector3 origin = transform.position;
        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.down, out hit, rayDistance + rayMargins, collisionLayers, QueryTriggerInteraction.Ignore)) {
            transform.Translate(0, -hit.distance + rayDistance, 0);
            if (isFalling) anim.Play("Land");
            isFalling = false;
        } else {
            transform.Translate(0, -Time.deltaTime * fallSpeed, 0);
            isFalling = true;
        }
    }

    protected void settle() {
        hasSettled = true;
    }

    

}

//#############################################################


public class BlockComparer : IComparer<Block> {
    public int Compare(Block x, Block y) {
        if (x.transform.position.y > y.transform.position.y) return 1;
        if (x.transform.position.y < y.transform.position.y) return -1;
        return 0;
    }
}
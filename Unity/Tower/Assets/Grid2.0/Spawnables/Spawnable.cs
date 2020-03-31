using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour{
    public static int spawnable = 0;
    public float fallSpeed = 6;
    [HideInInspector]
    public bool mobile = true;
    [HideInInspector]
    public bool isFalling = true;
    

    public LayerMask collisionLayers;

    /*
    [Header("Despawn Settings")]
    public bool invincible = false;
    public LayerMask upwardsCollisionMask;
    [Range(0, 1)]
    public float upwardsCollisionDetection = 0.5f;*/

    protected int id = 0;
    protected float rayMargins = 0.01f;
    protected float raySupDistance = 0.5f;
    protected float rayDistance;
    protected RaycastHit[] hitBuffer = new RaycastHit[16];
    protected Vector2Int col;
    protected Animation anim;

    private void Start() {
        id = spawnable++;
        anim = gameObject.GetComponentInChildren<Animation>();
        rayDistance = GetComponent<Collider>().bounds.extents.y;
        GetComponent<Renderer>();
    }

    private void FixedUpdate() {
        if (mobile) tryFall(); //If allowed try to fall
        upwardCollisionCheck();
    }


    public void reset(Vector2Int col) {
        this.col = col;
        mobile = true;
        isFalling = true;
        gameObject.SetActive(true);
    }


    protected void tryFall() {
        Vector3 origin = transform.position;
        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.down, out hit, rayDistance + rayMargins, collisionLayers, QueryTriggerInteraction.Ignore)) {
            transform.Translate(0, -hit.distance + rayDistance, 0);
            if(isFalling) anim.Play("Bobbing");
            isFalling = false;
        } else {
            transform.Translate(0, -Time.deltaTime * fallSpeed, 0);
            isFalling = true;
        }
    }

    protected void upwardCollisionCheck() {
        Vector3 origin = transform.position;
        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.up, out hit, rayDistance + rayMargins, collisionLayers, QueryTriggerInteraction.Ignore)) {
            GetComponentInParent<Grid>().spawnableRemoved(col);
            Destroy(gameObject);
        }  
    }

/*
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && !isFalling) {
            GetComponentInParent<Grid>().spawnableUsed(col);
            Destroy(gameObject);
        }
    }*/

    

}

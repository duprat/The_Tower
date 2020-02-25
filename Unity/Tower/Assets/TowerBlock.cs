using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerBlock : MonoBehaviour {
    public float dimension = 1;
    public bool mobile = true;
    public float fallSpeed = 1;
    public LayerMask collisionLayers;
    public bool isFalling = true;
    protected float rayMargins = 0.01f;
    protected float rayDistance;

    public abstract void spawn(Vector3 position);
    public abstract void kill();

    private void Start() {
        rayDistance = GetComponent<Collider>().bounds.extents.y;
    }

    private void FixedUpdate(){
        tryFall();
    }

    protected void tryFall(){
        if(mobile){
            Vector3 origin = transform.position; //+ (Vector3.down * (dimension / 2.0f));
            RaycastHit hit;
            if (Physics.Raycast(origin, Vector3.down, out hit, rayDistance + rayMargins, collisionLayers, QueryTriggerInteraction.Ignore)) {
                transform.Translate(0, -hit.distance + rayDistance, 0);
                isFalling = false;
            } else {
                transform.Translate(0, -Time.deltaTime * fallSpeed, 0);
                isFalling = true;
            }
        }
    }
}

//#############################################################


public class TowerBlockComparer : IComparer<TowerBlock> {
    public int Compare(TowerBlock x, TowerBlock y) {
        if (x.transform.position.y > y.transform.position.y) return 1;
        if (x.transform.position.y < y.transform.position.y) return -1;
        return 0;
    }
}

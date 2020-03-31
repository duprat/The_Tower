using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Spawnable{
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Player") && !isFalling) {
            GetComponentInParent<Grid>().spawnableUsed(col);
            Destroy(gameObject);
        }
    }

}

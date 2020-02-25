using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTowerBlock : TowerBlock
{
    public override void kill() {
        GameObject.Destroy(transform.gameObject);
    }

    public override void spawn(Vector3 position) {
        mobile = true;
    }
}

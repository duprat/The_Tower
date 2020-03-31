using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Generator : MonoBehaviour{

    public bool active = true;

    public float interval = 1;

    public List<GameObject> spawnableItems = new List<GameObject>();

    protected float timer = 0;

    protected GridManager gridManager;



    // Start is called before the first frame update
    void Awake() {
        gridManager = GetComponent<GridManager>();
    }

    public abstract void generate();

    protected Orientation getPlayerSide() {
        return gridManager.getPlayerCurrentSide();
    }

    protected Vector3Int getPlayerBlock() { // 0;0 -> FORWARD-RIGHT corner
        return gridManager.getPlayerCurrentBlock();
    }
}

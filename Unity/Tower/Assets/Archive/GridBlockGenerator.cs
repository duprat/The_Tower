using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridBlockGenerator : MonoBehaviour{

    public float interval = 1;
    protected float timer = 0;
    protected TowerGrid tg;

    private GridPlayerManager gpm;
    

    
    // Start is called before the first frame update
    void Start(){
        gpm = GetComponent<GridPlayerManager>();
        tg = GetComponent<TowerGrid>();
    }

    protected Orientation getPlayerSide() {
       return gpm.getPlayerCurrentSide();
    }

    protected Vector2Int getPlayerBlock() { // 0;0 -> FORWARD-RIGHT corner
        return gpm.getPlayerCurrentBlock();
    }

    protected void spawnBlock(int x, int y) {
        tg.spawnBlock(new Vector2Int(x, y));
    }
    protected void spawnBlock()
    {
        tg.spawnRandom();
    }
}

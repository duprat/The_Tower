using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerColumn : MonoBehaviour{
    public float dimension;
    public GameObject blockBlueprint;
    public float spawnAltitude = 10;
    protected List<TowerBlock> blocks = new List<TowerBlock>(); 
    private List<TowerBlock> blockBuffer = new List<TowerBlock>();

    // Start is called before the first frame update
    void Awake(){
        dimension = blockBlueprint.GetComponent<TowerBlock>().dimension;
        foreach(TowerBlock tb in GetComponentsInChildren<TowerBlock>()) blocks.Add(tb);
    }

    public void trimHeight(int levels) {
        if (levels > getHeight()) return;
        blocks[levels].mobile = false;
        for (int i = 0; i < levels; i++) {
            TowerBlock removable = blocks[0];
            blocks.RemoveAt(0);
            removable.transform.parent = null;
            removable.gameObject.SetActive(false);
            blockBuffer.Add(removable);
        }
    }

    public bool spawnBlock(){
        if (blocks.Count > 0 && blocks[blocks.Count - 1].isFalling) return false;  // VERIFIED?
        if (blockBuffer.Count > 0) { blocks.Add(blockBuffer[0]); blockBuffer.RemoveAt(0); }
        else blocks.Add(Instantiate(blockBlueprint).GetComponent<TowerBlock>());
        blocks[blocks.Count - 1].transform.position = transform.position + Vector3.up * getCurrentSpawnHeight();
        blocks[blocks.Count - 1].transform.rotation = transform.rotation;
        blocks[blocks.Count - 1].transform.parent = transform;
        blocks[blocks.Count - 1].mobile = true;
        blocks[blocks.Count - 1].gameObject.SetActive(true);
        return true;
    }

    protected float getCurrentSpawnHeight() {
        if(blocks.Count < 1) return getHeight() * dimension + spawnAltitude;
        return blocks[blocks.Count-1].transform.localPosition.y + dimension + spawnAltitude;
    }

    public int getHeight() {
        return blocks.Count;
    }

    public void reset() {
        dimension = blockBlueprint.GetComponent<TowerBlock>().dimension;
        while (blocks.Count > 0) {
            TowerBlock removable = blocks[0];
            blocks.RemoveAt(0);
            GameObject.Destroy(removable.gameObject);
        }
        Debug.Log(blocks.Count);
        while (blockBuffer.Count > 0) {
            TowerBlock removable = blockBuffer[0];
            blockBuffer.RemoveAt(0);
            GameObject.Destroy(removable.gameObject);
        }
    }

}

//#############################################################


public class TowerColumnComparer : IComparer<TowerColumn> {
    public int Compare(TowerColumn x, TowerColumn y) {
        if (x.getHeight() == y.getHeight()) return 0;
        if (x.getHeight() > y.getHeight()) return 1;
        return -1;
    }
}

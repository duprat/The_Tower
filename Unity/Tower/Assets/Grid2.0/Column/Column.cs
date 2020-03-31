using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour{
    public GameObject blockBlueprint;

    protected Spawnable spawnable;
    protected List<Block> blocks = new List<Block>();
    protected List<Block> blockBuffer = new List<Block>();
    protected int stackedBlocks = 0;

    public void displace(Vector3 displacement) {
        foreach (Block block in blocks) block.transform.position += displacement;
        //transform.position += displacement;
    }

    public void initializeColumn(int initialThickness) {
        reset(initialThickness);
    }

    public void trim(int blocksToRemove) { //removes blocksToRemove from the bottom and immobilises the above
        for (int i = 0; i < blocksToRemove; i++) fixedBreakBlock(0);
    }

    public int getHeight() { //returns number of blocks that have stacked including updating ones
        updateStackedBlocks();
        return stackedBlocks;
    }

    /*public GameObject[] getFullRepresentation() {
        GameObject[] representation 
    }*/

    public void spawnSpawnable(Spawnable o, float height) {
        spawnable = o;
        spawnable.transform.position = transform.position + Vector3.up * height;
        spawnable.transform.rotation = transform.rotation;
        spawnable.transform.parent = transform;
        spawnable.mobile = true;
        spawnable.gameObject.SetActive(true);
    }

    public bool spawnBlocks(int amount, float spawnAltitude) {
        float lowerBound = transform.position.y + spawnAltitude - 1 ; // Checking for overlapping blocks 
        if (blocks[blocks.Count - 1].transform.position.y > lowerBound) return false;
        for (int i = 0; i < amount; i++) spawnBlock(spawnAltitude + i);
        return true;
    }

    public void explode(int height, int radius) {
        for (int i = -radius; i <= radius; i++) dynamicBreakBlock(height+i);
    }

    //###########################################

    protected void spawnBlock(float height) {
        if (blockBuffer.Count > 0) { blocks.Add(blockBuffer[0]); blockBuffer.RemoveAt(0); } else blocks.Add(Instantiate(blockBlueprint).GetComponent<Block>());
        blocks[blocks.Count - 1].transform.position = transform.position + Vector3.up * height;
        blocks[blocks.Count - 1].transform.rotation = transform.rotation;
        blocks[blocks.Count - 1].transform.parent = transform;
        blocks[blocks.Count - 1].reset();
    }

    protected void updateStackedBlocks() {
        stackedBlocks = 0;
        for (int i = 0; i < blocks.Count; i++) {
            if (blocks[i].hasSettled) {
                blocks[i].mobile = false;
                stackedBlocks++;
            }
        }
    }

    protected void reset(int initialThickness) {
        clearBlocks();
        clearBuffer();
        setFirstBlocks(initialThickness);
    }

    protected void setFirstBlocks(int numberOfBlocks) {
        for (int i = 0; i < numberOfBlocks; i++) {
            blocks.Add(Instantiate(blockBlueprint).GetComponent<Block>());
            blocks[i].reset();
            blocks[i].transform.position = transform.position + Vector3.up * i;
            blocks[i].transform.rotation = transform.rotation;
            blocks[i].transform.parent = transform;
            blocks[i].hasSettled = true;
            blocks[i].isFalling = false;
            blocks[i].mobile = false;
            blocks[i].gameObject.SetActive(true);
        }
    }

    protected void dynamicBreakBlock(int i) { //Sends specific block to the buffer and allows above ones to fall
        if (i >= blocks.Count || i < 0) return;
        Block removed = blocks[i];
        blocks.RemoveAt(i);
        removed.transform.parent = null;
        removed.explode();
        blockBuffer.Add(removed);
        for (int y = i; y < blocks.Count; y++) {
            blocks[y].reset();
        }
    }

    protected void fixedBreakBlock(int i) { //Sends specific block to the buffer and allows refill
        Block removed = blocks[i];
        blocks.RemoveAt(i);
        removed.transform.parent = null;
        removed.gameObject.SetActive(false);
        blockBuffer.Add(removed);
        blocks[i].hasSettled = true;
    }

    protected void clearBuffer() { //Destroys all buffered blocks
        while (blockBuffer.Count > 0) {
            Block removed = blockBuffer[0];
            blockBuffer.RemoveAt(0);
            removed.transform.parent = null;
            Destroy(removed.gameObject);
        }
    }

    protected void clearBlocks() { //Destroys all blocks
        while (blocks.Count > 0) {
            Block removed = blocks[0];
            blockBuffer.RemoveAt(0);
            removed.transform.parent = null;
            Destroy(removed.gameObject);
        }
    }
}


//###########################################################


public class ColumnComparer : IComparer<Column> {
    public int Compare(Column x, Column y) {
        if (x.getHeight() == y.getHeight()) return 0;
        if (x.getHeight() > y.getHeight()) return 1;
        return -1;
    }
}

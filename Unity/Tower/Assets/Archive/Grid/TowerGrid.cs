using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGrid : MonoBehaviour{
    protected Vector2 dimensions; 

    public float individualColumnDimension = 0;
    public Vector2 centeredDimensions;
    public Vector2 center = Vector2.zero;
    public int heightThreshold = 10;
    public int gridSize = 5;
    public GameObject columnBlueprint;
    public TowerColumn[,] columns; //local colum positions, global = center * columnPositions
    public List<TowerColumn> sortedColums;

    void Start(){
        center = new Vector2(transform.position.x, transform.position.z);
        createColumns();
    }

    public List<TowerColumn> getSortedColumns() {
        return sortedColums;
    }

    void bottomManagement() {
        int shortest = sortedColums[0].getHeight();
        if ( shortest > heightThreshold) {
            foreach (TowerColumn column in sortedColums) {
                column.trimHeight(shortest - heightThreshold);
            }
        }
    }

    void updateColumnHeightSort() { // to be called at each block spawn on a column
        sortedColums.Sort(new TowerColumnComparer());
    }

    void createColumns(){  // Spawniong, placing and childing columns, called once
        columns = new TowerColumn[gridSize, gridSize];
        sortedColums = new List<TowerColumn>();
        for (int x = 0; x < gridSize; x++) {
            addColumn(x, 0);
            addColumn(x, gridSize - 1);
        }
        for (int z = 1; z < gridSize-1; z++) {
            addColumn(0, z);
            addColumn(gridSize - 1, z);
        }
        individualColumnDimension = columns[0, 0].dimension;
        dimensions.x = dimensions.y = gridSize * individualColumnDimension;
        centeredDimensions.x = centeredDimensions.y = gridSize * individualColumnDimension - individualColumnDimension;
    }

    void addColumn(int x, int z) {
        columns[x, z] = Instantiate(columnBlueprint).GetComponent<TowerColumn>();
        float xPos = (x - (gridSize / 2.0f) + 0.5f) * columns[x, z].dimension;
        float zPos = (z - (gridSize / 2.0f) + 0.5f) * columns[x, z].dimension;
        columns[x, z].transform.position = new Vector3(xPos, transform.position.y, zPos);
        columns[x, z].transform.rotation = transform.rotation;
        columns[x, z].transform.parent = transform;
        columns[x, z].name = "col" + x + ":" + z;
        sortedColums.Add(columns[x, z]);
    }

    public void reset() {
        foreach (TowerColumn tc in sortedColums) {
            Destroy(tc.gameObject);
        }
        createColumns();
    }

    public void spawnBlock(Vector2Int col) {
        columns[(int)col.x,(int)col.y].spawnBlock();
        updateColumnHeightSort();
        bottomManagement();
    }

    public void spawnRandom() {
        sortedColums[Random.Range(0, sortedColums.Count)].spawnBlock();
        updateColumnHeightSort();
        bottomManagement();
    }

}

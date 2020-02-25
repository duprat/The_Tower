using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGrid : MonoBehaviour{
    protected Vector2 dimensions; 
    protected float individualColumnDimension = 0;
    protected float timePassed = 0;
    protected float gridDiameter;

    public Vector2 centeredDimensions;
    public Vector2 center = Vector2.zero;
    public int heightThreshold = 10;
    public int scale = 5;
    public GameObject columnBlueprint;
    public TowerColumn[,] columns; //local colum positions, global = center * columnPositions
    public List<TowerColumn> sortedColums;
    public float timer = 0;

    void Start(){
        center = new Vector2(transform.position.x, transform.position.z);
        createColumns();
    }

    void Update(){
        timePassed += Time.deltaTime;
        if (timePassed >= timer) {
            timePassed = 0;
            spawnRandom();
        }
        
    }

    public List<TowerColumn> getSortedColumns() {
        return sortedColums;
    }

    void bottomManagement() {
        int shortest = sortedColums[0].getHeight();
        Debug.Log(shortest + " / " + heightThreshold);
        if ( shortest > heightThreshold) {
            foreach (TowerColumn column in sortedColums) {
                column.trimHeight(shortest - heightThreshold);
            }
        }
    }

    void updateColumnHeightSort() { // to be call at each block spawn on a column
        sortedColums.Sort(new TowerColumnComparer());
    }

    void createColumns(){  // Spawniong, placing and childing columns, called once
        columns = new TowerColumn[scale, scale];
        sortedColums = new List<TowerColumn>();
        for (int x = 0; x < scale; x++) {
            addColumn(x, 0);
            addColumn(x, scale - 1);
        }
        for (int z = 1; z < scale-1; z++) {
            addColumn(0, z);
            addColumn(scale - 1, z);
        }
        individualColumnDimension = columns[0, 0].dimension;
        dimensions.x = dimensions.y = scale * individualColumnDimension;
        centeredDimensions.x = centeredDimensions.y = scale * individualColumnDimension - individualColumnDimension;
    }

    void addColumn(int x, int z) {
        columns[x, z] = Instantiate(columnBlueprint).GetComponent<TowerColumn>();
        float xPos = (x - (scale / 2.0f) + 0.5f) * columns[x, z].dimension;
        float zPos = (z - (scale / 2.0f) + 0.5f) * columns[x, z].dimension;
        columns[x, z].transform.position = new Vector3(xPos, transform.position.y, zPos);
        columns[x, z].transform.rotation = transform.rotation;
        columns[x, z].transform.parent = transform;
        columns[x, z].name = "col" + x + ":" + z;
        sortedColums.Add(columns[x, z]);
    }

    void spawnRandom() {
        sortedColums[Random.Range(0, sortedColums.Count)].spawnBlock();
        updateColumnHeightSort();
        bottomManagement();
    }
}

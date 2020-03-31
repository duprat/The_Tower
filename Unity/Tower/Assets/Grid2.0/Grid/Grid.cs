using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    [HideInInspector]
    public Vector3 center = Vector3.zero; //Center of grid in real world coordinates
    [HideInInspector]
    public float gridCenterToBlockCenter; //Spacing between center and block center

    [Header("Initialization Settings")]
    public int initialThickness = 5;
    public int gridSize = 5; //Square grid dimendion in number of blocks
    public GameObject columnBlueprint; //Column type to be spawned

    [Header("Mechanics")]
    public bool autoMinimize = true; //Tower auto shortening feature by removing useless blocks
    public int minimizationThreshold = 10; //Threshold used if auto minimize is on (excludes falling blocks)
    public float spawnAltitude = 20;
    public int explosionRadius = 1;


    protected Spawnable[,] positionedSpawnables; //Spawnables in real world arrangement
    protected Column[,] positionedColumns; //Columns in real world arrangement
    protected List<Column> heightSortedColumns = new List<Column>(); //Height sorted columns ascending order
    protected List<Column> sortedColumns = new List<Column>(); //Ordered columns starting from FORWARD-LEFT corner and clockwise

    private void FixedUpdate() {
        updateColumnHeightSort();
    }

    public List<Column> getHeightSortedColumns() { return heightSortedColumns; }
    public List<Column> getSortedColumns() { return sortedColumns; }
    public Column[,] getColumns() { return positionedColumns; }
    public int getTallest() { return heightSortedColumns[heightSortedColumns.Count - 1].getHeight(); }
    public int getShortest() { return heightSortedColumns[0].getHeight(); }

    public void initializeGrid() { // Resets the grid to starting position\
        transform.position = Vector3.zero;
        center = transform.position;
        gridCenterToBlockCenter = (gridSize - 1)/ 2.0f;
        reset();
    }

    public bool breakBlockAt(Vector2Int positionInRepresentation) {
        return false;
    }

    public bool spawnSpawnable(GameObject o) {
        Vector2Int col = new Vector2Int(Random.Range(0, gridSize), Random.Range(0, gridSize));
        if (Random.Range(0, 2) == 0) col.x = Random.Range(0, 2) == 0 ? 0 : gridSize - 1;
        else col.y = Random.Range(0, 2) == 0 ? 0 : gridSize - 1;
        return spawnSpawnable(o, col);
    }

    public bool spawnSpawnable(GameObject o, Vector2Int col) {
        if (positionedSpawnables[col.x, col.y] != null) return false;
        positionedSpawnables[col.x, col.y] = Instantiate(o).GetComponent<Spawnable>();
        positionedSpawnables[col.x, col.y].reset(col);
        positionedColumns[col.x, col.y].spawnSpawnable(positionedSpawnables[col.x, col.y], getTallest() + spawnAltitude);
        return true;
    }

    public bool spawnBlocks(Vector2Int col, int amount) {
        if (positionedColumns[col.x, col.y].spawnBlocks(amount, getTallest() + spawnAltitude)) {
            if(autoMinimize)minimize();
            return true;
        }
        return false;
    }

    public bool spawnBlocks() {
        if (sortedColumns[Random.Range(0, sortedColumns.Count)].spawnBlocks(1, getTallest() + spawnAltitude)) {
            if (autoMinimize) minimize();
            return true;
        }
        return false;
    }


    public void spawnableUsed(Vector2Int col) {
        //Temporary explosion effect #################################
        int height = positionedColumns[col.x, col.y].getHeight();
        for (int x = -explosionRadius; x <= explosionRadius; x++) for (int y = -explosionRadius; y <= explosionRadius; y++) {
                if (isOnEdge(new Vector2Int(col.x + x, col.y + y))) {
                    positionedColumns[col.x + x, col.y + y].explode(height, explosionRadius);
                }
            }
        //#############################################################
        positionedSpawnables[col.x, col.y] = null;
    }

    public void spawnableRemoved(Vector2Int col) {
        positionedSpawnables[col.x, col.y] = null;
    }

    //###################################################
    protected bool isOnEdge(Vector2Int col) {
        return (col.y >= 0 && col.y < gridSize && col.x >= 0 && col.x < gridSize) && (col.y == 0 || col.y == gridSize - 1 || col.x == 0 || col.x == gridSize - 1);
    }

    protected void updateColumnHeightSort() {
        heightSortedColumns.Sort(new ColumnComparer());
    }

    protected void reset() {
        foreach (Column tc in sortedColumns) {
            Destroy(tc.gameObject);
        }
        sortedColumns.Clear();
        heightSortedColumns.Clear();
        positionedColumns = null;
        createColumns();
    }

    protected void minimize() {
        if (getShortest() > minimizationThreshold) {           //It's taller than the threshold 
            int amountOfBlocksToRemove = getShortest() - minimizationThreshold;
            foreach (Column column in sortedColumns) {
                column.trim(amountOfBlocksToRemove);              //Cut every column
            }
            displace(Vector3.down * amountOfBlocksToRemove); //Replace everithing back down
        }
    }

    protected void minimize(int threshold) {
        int shortest = sortedColumns[0].getHeight();
        if (shortest > threshold) {
            foreach (Column column in sortedColumns) {
                column.trim(shortest - threshold);
            }
        }
    }

    protected void displace(Vector3 displacement) {
        foreach (Column column in sortedColumns) column.displace(displacement);
        //transform.position += displacement;
        //center = transform.position;
    }

    protected void createColumns() {  // Spawning, placing and childing columns
        positionedColumns = new Column[gridSize, gridSize];
        positionedSpawnables = new Spawnable[gridSize, gridSize];
        sortedColumns = new List<Column>();
        heightSortedColumns = new List<Column>();
        //FOWARDFACE
        for (int x = 0; x < gridSize; x++) addColumn(x, 0);
        //RIGHT FACE
        for(int z = 1; z < gridSize - 1; z++) addColumn(0, z);
        //BACK FACE
        for (int x = 0; x < gridSize; x++) addColumn(x, gridSize - 1);
        //LEFT FACE   
        for (int z = 1; z < gridSize - 1; z++) addColumn(gridSize - 1, z);
        updateColumnHeightSort();
    }

    protected void addColumn(int x, int z) {
        positionedColumns[x, z] = Instantiate(columnBlueprint).GetComponent<Column>();
        positionedColumns[x, z].transform.rotation = transform.rotation; //Reseting rotation
        positionedColumns[x, z].transform.position = center + Vector3.forward * (z - gridCenterToBlockCenter) + Vector3.right * (x - gridCenterToBlockCenter); // Repositioning to grid
        positionedColumns[x, z].transform.parent = transform; //Setting column as child
        positionedColumns[x, z].name = "col" + x + ":" + z; //Renaming (not necessary)
        sortedColumns.Add(positionedColumns[x, z]); //Adding references 
        heightSortedColumns.Add(positionedColumns[x, z]);
        positionedColumns[x, z].initializeColumn(initialThickness);
        positionedSpawnables[x, z] = null;
    }

}



/*public GameObject[][] getFullRepresentation(bool includeFallingBlocks, bool includeItems) { //A full 2D representation of the current state starting from the FRONT-LEFT corner and clockwise. 0 is ground.
    GameObject[][] representation = new GameObject[sortedColums.Count][]; //Initialise to [nb columns]
    for (int i = 0; i < sortedColums.Count; i++) representation[i] = sortedColums[i].getFullRepresentation(includeFallingBlocks, includeItems); // Fill each column
    return representation;
}*/

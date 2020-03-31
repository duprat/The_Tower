using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    [HideInInspector]
    public bool initialized = false;
    [HideInInspector]
    public bool active = false;
    [HideInInspector]
    public bool hasAssignedPlayer = false;
    [HideInInspector]
    public bool hasCinematicCamera = false;

    [Header("Player Control")]
    [Range(0,0.5f)]
    public float sideChangeBuffer = 0.05f;
    [Range(0, 1)]
    public float cornerSmoothingBuffer = 1f;

    protected PlayerManager player;
    protected Vector3Int playerCurrentBlock;
    protected Orientation playerCurrentSide;
    protected Grid grid;
    protected Generator[] generators;
    protected Vector2 gridBoundsMax; // X and Y positive bounds defined by grid corner cube center - cornerSmoothingBuffer. Used for player-in-corner detection.
    protected Vector2 gridBoundsMin; // X and Y negative bounds defined by grid corner cube center + cornerSmoothingBuffer
    protected CinematicCamera cCamera;
    protected InGameCamera[] igCameras;
    protected int activeIGCamera = 0;

    private void Awake() {
        GetComponentInChildren<Camera>().gameObject.SetActive(true);
        grid = GetComponent<Grid>();
        generators = GetComponents<Generator>();
        cCamera = GetComponent<CinematicCamera>();
        igCameras = GetComponents<InGameCamera>();
        cCamera.setInitialValues();
        grid.initializeGrid();
        gridBoundsMax = new Vector2(grid.center.x + grid.gridCenterToBlockCenter - cornerSmoothingBuffer, grid.center.z + grid.gridCenterToBlockCenter - cornerSmoothingBuffer);
        gridBoundsMin = new Vector2(grid.center.x - grid.gridCenterToBlockCenter + cornerSmoothingBuffer, grid.center.z - grid.gridCenterToBlockCenter + cornerSmoothingBuffer);
        initialized = true;
    }

    private void Start() {
        GameEvents.current.onChangeCamera += changeCamera;
    }

    private void FixedUpdate() {    //Active loop
        
        if (active)
        {
            foreach (Generator gen in generators) if(gen.active)gen.generate(); //Generation call
        }

        if (hasAssignedPlayer && player.hasMoved())
        {
            playerSideUpdate();
            playerBlockUpdate();
            igCameras[activeIGCamera].updateCameraPosition();
        }

        if (!hasAssignedPlayer){
            cCamera.updateCameraPosition();
        }
    }

    public Orientation getPlayerCurrentSide() {
        return playerCurrentSide;
    }
    public Vector3Int getPlayerCurrentBlock() {
        return playerCurrentBlock;
    }

    public bool spawnBlocks(Vector2Int pos, int amount) {
        return grid.spawnBlocks(pos, amount);
    }

    public bool spawnBlocks() {
        return grid.spawnBlocks();
    }

    public bool spawnSpawnable(GameObject o) {
        return grid.spawnSpawnable(o);
    }

    public bool spawnSpawnable(GameObject o, Vector2Int pos) {
        return grid.spawnSpawnable(o, pos);
    }

    public GameObject getPlayer() { return player.gameObject; }

    public bool setPlayer(PlayerManager pc) {
        if (hasAssignedPlayer) return false;
        player = pc;
        hasAssignedPlayer = true;
        placePlayerRandom();
        active = true;
        igCameras[activeIGCamera].setInitialValues();
        return true;
    }

    public void removePlayer() {
        player = null;
        hasAssignedPlayer = false;
        grid.initializeGrid();
        cCamera.setInitialValues();
        active = false;
    }

    public int getGridSize() {
        return grid.gridSize;
    }

    //############################################################
    protected void changeCamera() {
        if (activeIGCamera == igCameras.Length - 1) activeIGCamera = 0;
        else activeIGCamera++;
        igCameras[activeIGCamera].setInitialValues();
        igCameras[activeIGCamera].updateCameraPosition();
    }

    protected void placePlayerRandom() {
        float playerHalfSize = player.GetComponent<Collider>().bounds.size.y + grid.initialThickness + 1;
        player.moveTo(grid.getSortedColumns()[Random.Range(0, grid.getSortedColumns().Count)].transform.position + Vector3.up * playerHalfSize);
        setPlayerCurrentSide();
    }

    protected void setPlayerCurrentSide() {
        if (player.getPosition().z >= grid.center.z + grid.gridCenterToBlockCenter) playerCurrentSide = Orientation.FORWARD;
        else if (player.getPosition().z <= grid.center.z - grid.gridCenterToBlockCenter) playerCurrentSide = Orientation.BACK;
        else if (player.getPosition().x >= grid.center.x + grid.gridCenterToBlockCenter) playerCurrentSide = Orientation.RIGHT;
        else if (player.getPosition().x <= grid.center.x + grid.gridCenterToBlockCenter) playerCurrentSide = Orientation.LEFT;
        setPlayerFacingAngle();
    }

    protected void setPlayerFacingAngle(float smoothie) {
        player.setAngle((float)playerCurrentSide, smoothie);
    }

    protected void setPlayerFacingAngle() {
        player.setAngle((float)playerCurrentSide, (float)playerCurrentSide);
    }

    protected void playerSideUpdate() {
        //Checking if player is out of within a corner (cornerSmoothingBuffer distance from an edge) in x and y direction.
        float xDeviation = inRange(player.getPosition().x, gridBoundsMin.x, gridBoundsMax.x); //Debug.Log("X" + xDeviation);
        float zDeviation = inRange(player.getPosition().z, gridBoundsMin.y, gridBoundsMax.y); //Debug.Log("Z" + zDeviation);
        //Perform actions depending on current side:
        switch (playerCurrentSide) {
            case Orientation.FORWARD:
                if (xDeviation > 0) { // Positive deviation means player is on the right corner of forward face
                    if (Mathf.Abs(xDeviation) > cornerSmoothingBuffer + sideChangeBuffer) { //If player is over the sideChangeBuffer perform side change
                        player.moveTo(new Vector3(grid.center.x + grid.gridCenterToBlockCenter, player.getPosition().y, player.getPosition().z));
                        playerCurrentSide = Orientation.RIGHT;
                    }//adjust smoothing angle to corner proximity
                    float smoothieAngle = (float)Orientation.FORWARD + ((Mathf.Abs(xDeviation) / cornerSmoothingBuffer - sideChangeBuffer) * 45);
                    setPlayerFacingAngle(smoothieAngle);
                    //Debug.Log("From forward to right");

                } else if (xDeviation < 0) {// Negative deviation means player is on the left corner of forward face
                    if (Mathf.Abs(xDeviation) > cornerSmoothingBuffer + sideChangeBuffer) {
                        player.moveTo(new Vector3(grid.center.x - grid.gridCenterToBlockCenter, player.getPosition().y, player.getPosition().z));
                        playerCurrentSide = Orientation.LEFT;
                    }// else adjust smoothing angle to corner proximity
                    float smoothieAngle = (float)Orientation.FORWARD - ((Mathf.Abs(xDeviation) / cornerSmoothingBuffer - sideChangeBuffer) * 45);
                    setPlayerFacingAngle(smoothieAngle);
                    //Debug.Log("From forward to left");
                } else {
                    setPlayerCurrentSide();
                }
                break;
            case Orientation.BACK:
                if (xDeviation > 0) { // Positive deviation means player is on the right corner of forward face
                    if (Mathf.Abs(xDeviation) > cornerSmoothingBuffer + sideChangeBuffer) { //If player is over the sideChangeBuffer perform side change
                        player.moveTo(new Vector3(grid.center.x + grid.gridCenterToBlockCenter, player.getPosition().y, player.getPosition().z));
                        playerCurrentSide = Orientation.RIGHT;
                    }// adjust smoothing angle to corner proximity
                    float smoothieAngle = (float)Orientation.BACK - ((Mathf.Abs(xDeviation) / cornerSmoothingBuffer - sideChangeBuffer) * 45);
                    setPlayerFacingAngle(smoothieAngle);
                    //Debug.Log("From back to right");

                } else if (xDeviation < 0) {// Negative deviation means player is on the left corner of forward face
                    if (Mathf.Abs(xDeviation) > cornerSmoothingBuffer + sideChangeBuffer) {
                        player.moveTo(new Vector3(grid.center.x - grid.gridCenterToBlockCenter, player.getPosition().y, player.getPosition().z));
                        playerCurrentSide = Orientation.LEFT;
                    }// adjust smoothing angle to corner proximity
                    float smoothieAngle = (float)Orientation.BACK + ((Mathf.Abs(xDeviation) / cornerSmoothingBuffer - sideChangeBuffer) * 45);
                    setPlayerFacingAngle(smoothieAngle);
                    //Debug.Log("From back to left");
                } else {
                    setPlayerCurrentSide();
                }
                break;
            case Orientation.RIGHT:
                if (zDeviation > 0) {
                    if (Mathf.Abs(zDeviation) > cornerSmoothingBuffer + sideChangeBuffer) {
                        player.moveTo(new Vector3(player.getPosition().x, player.getPosition().y, grid.center.z + grid.gridCenterToBlockCenter));
                        playerCurrentSide = Orientation.FORWARD;
                    }
                    float smoothieAngle = (float)Orientation.RIGHT - ((Mathf.Abs(zDeviation) / cornerSmoothingBuffer - sideChangeBuffer) * 45);
                    setPlayerFacingAngle(smoothieAngle);
                    //Debug.Log("From right to forward");
                } else if (zDeviation < 0) {
                    if (Mathf.Abs(zDeviation) > cornerSmoothingBuffer + sideChangeBuffer) {
                        player.moveTo(new Vector3(player.getPosition().x, player.getPosition().y, grid.center.z - grid.gridCenterToBlockCenter));
                        playerCurrentSide = Orientation.BACK;
                    }
                    float smoothieAngle = (float)Orientation.RIGHT + ((Mathf.Abs(zDeviation) / cornerSmoothingBuffer - sideChangeBuffer) * 45);
                    setPlayerFacingAngle(smoothieAngle);
                    //Debug.Log("From right to back");
                } else {
                    setPlayerCurrentSide();
                }
                break;
            case Orientation.LEFT:
                if (zDeviation > 0) {
                    if (Mathf.Abs(zDeviation) > cornerSmoothingBuffer + sideChangeBuffer) {
                        player.moveTo(new Vector3(player.getPosition().x, player.getPosition().y, grid.center.z + grid.gridCenterToBlockCenter));
                        playerCurrentSide = Orientation.FORWARD;
                    }
                    float smoothieAngle = (float)Orientation.LEFT + ((Mathf.Abs(zDeviation) / cornerSmoothingBuffer - sideChangeBuffer) * 45);
                    setPlayerFacingAngle(smoothieAngle);
                    //Debug.Log("From left to forward");
                } else if (zDeviation < 0) {
                    if (Mathf.Abs(zDeviation) > cornerSmoothingBuffer + sideChangeBuffer) {
                        player.moveTo(new Vector3(player.getPosition().x, player.getPosition().y, grid.center.z - grid.gridCenterToBlockCenter));
                        playerCurrentSide = Orientation.BACK;
                    }
                    float smoothieAngle = (float)Orientation.LEFT - ((Mathf.Abs(zDeviation) / cornerSmoothingBuffer - sideChangeBuffer) * 45);
                    setPlayerFacingAngle(smoothieAngle);
                    //Debug.Log("From left to back");
                } else {
                    setPlayerCurrentSide();
                }
                break;
        }
    }

    protected float inRange(float x, float min, float max) {
        if (x < min) return x - min;
        if (x > max) return x - max;
        return 0;
    }

    protected void playerBlockUpdate() {
        playerCurrentBlock.x = grid.gridSize - Mathf.CeilToInt(gridBoundsMax.x - player.getPosition().x );
        playerCurrentBlock.y = Mathf.CeilToInt(player.getPosition().y);
        playerCurrentBlock.z = grid.gridSize - Mathf.CeilToInt(gridBoundsMax.y - player.getPosition().z); 
    }

    
}

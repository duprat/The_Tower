using UnityEngine;

public class GridPlayerManager : MonoBehaviour{
    protected bool hasAssignedPlayer = false;
    protected TowerGrid grid;
    protected PlayerController player;
    protected Orientation playerCurrentSide;
    protected Vector2Int playerCurrentBlock;
    protected Vector2 gridBoundsMax; // X and Y positive bounds defined by grid corner cube center - cornerSmoothingBuffer. Used for player-in-corner detection.
    protected Vector2 gridBoundsMin; // X and Y negative bounds defined by grid corner cube center + cornerSmoothingBuffer

    public float sideChangeBuffer = 0.05f;
    public float cornerSmoothingBuffer = 1f;

    

    private void Start() {
        grid = GetComponent<TowerGrid>();
    }


    private void FixedUpdate() {
        if (hasAssignedPlayer && player.hasMoved){
            playerSideChangeCheck();
            playerBlockChangeCheck();
        }
    }

    public void playerDied() {
        placePlayerRandom();
        grid.reset();
    }

    public bool setPlayer(PlayerController pc) {
        if (hasAssignedPlayer) return false;
        player = pc;
        hasAssignedPlayer = true;
        placePlayerRandom();
        return true;
    }

    public PlayerController removePlayer() {
        PlayerController pc = player;
        player = null;
        hasAssignedPlayer = false;
        return pc;
    }

    void placePlayerRandom() {
        float playerHalfSize = player.GetComponent<Collider>().bounds.size.y + 1;
        player.transform.position = grid.getSortedColumns()[Random.Range(0, grid.getSortedColumns().Count)].transform.position + Vector3.up * playerHalfSize;
        setPlayerCurrentSide();
    }

    void setPlayerCurrentSide() {
        if (player.transform.position.z >= grid.center.y + (grid.centeredDimensions.y / 2.0f)) playerCurrentSide = Orientation.FORWARD;
        else if (player.transform.position.z <= grid.center.y - (grid.centeredDimensions.y / 2.0f)) playerCurrentSide = Orientation.BACK;
        else if (player.transform.position.x >= grid.center.x + (grid.centeredDimensions.x / 2.0f)) playerCurrentSide = Orientation.RIGHT;
        else if (player.transform.position.x <= grid.center.x + (grid.centeredDimensions.x / 2.0f)) playerCurrentSide = Orientation.LEFT;
        setPlayerFacingAngle();
    }

    void setPlayerFacingAngle(float smoothie) {
        player.setAngle((float)playerCurrentSide, smoothie);
    }

    void setPlayerFacingAngle() {
        player.setAngle((float)playerCurrentSide, (float)playerCurrentSide);
    }

    void playerSideChangeCheck() {
        //Checking if player is out of within a corner (cornerSmoothingBuffer distance from an edge) in x and y direction.
        gridBoundsMax = new Vector2(grid.center.y + (grid.centeredDimensions.y / 2) - cornerSmoothingBuffer, grid.center.x + (grid.centeredDimensions.x / 2) - cornerSmoothingBuffer);
        gridBoundsMin = new Vector2(grid.center.y - (grid.centeredDimensions.y / 2) + cornerSmoothingBuffer, grid.center.x - (grid.centeredDimensions.x / 2) + cornerSmoothingBuffer);
        float xDeviation = inRange(player.transform.position.x, gridBoundsMin.x, gridBoundsMax.x);
        float zDeviation = inRange(player.transform.position.z, gridBoundsMin.y, gridBoundsMax.y);
        //Perform actions depending on current side:
        switch (playerCurrentSide) {
            case Orientation.FORWARD:
                if (xDeviation > 0) { // Positive deviation means player is on the right corner of forward face
                    if (Mathf.Abs(xDeviation) > cornerSmoothingBuffer + sideChangeBuffer) { //If player is over the sideChangeBuffer perform side change
                        player.transform.position = new Vector3(grid.center.x + (grid.centeredDimensions.x / 2), player.transform.position.y, player.transform.position.z);
                        playerCurrentSide = Orientation.RIGHT;
                    }//adjust smoothing angle to corner proximity
                    float smoothieAngle = (float)Orientation.FORWARD + ((Mathf.Abs(xDeviation) / cornerSmoothingBuffer - sideChangeBuffer) * 45);
                    setPlayerFacingAngle(smoothieAngle);
                    //Debug.Log("From forward to right");

                } else if (xDeviation < 0) {// Negative deviation means player is on the left corner of forward face
                    if (Mathf.Abs(xDeviation) > cornerSmoothingBuffer + sideChangeBuffer) {
                        player.transform.position = new Vector3(grid.center.x - (grid.centeredDimensions.x / 2), player.transform.position.y, player.transform.position.z);
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
                        player.transform.position = new Vector3(grid.center.x + (grid.centeredDimensions.x / 2), player.transform.position.y, player.transform.position.z);
                        playerCurrentSide = Orientation.RIGHT;
                    }// adjust smoothing angle to corner proximity
                    float smoothieAngle = (float)Orientation.BACK - ((Mathf.Abs(xDeviation) / cornerSmoothingBuffer - sideChangeBuffer) * 45);
                    setPlayerFacingAngle(smoothieAngle);
                    //Debug.Log("From back to right");

                } else if (xDeviation < 0) {// Negative deviation means player is on the left corner of forward face
                    if (Mathf.Abs(xDeviation) > cornerSmoothingBuffer + sideChangeBuffer) {
                        player.transform.position = new Vector3(grid.center.x - (grid.centeredDimensions.x / 2), player.transform.position.y, player.transform.position.z);
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
                        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, grid.center.y + (grid.centeredDimensions.y / 2));
                        playerCurrentSide = Orientation.FORWARD;
                    }
                    float smoothieAngle = (float)Orientation.RIGHT - ((Mathf.Abs(zDeviation) / cornerSmoothingBuffer - sideChangeBuffer) * 45);
                    setPlayerFacingAngle(smoothieAngle);
                    //Debug.Log("From right to forward");
                } else if (zDeviation < 0) {
                    if (Mathf.Abs(zDeviation) > cornerSmoothingBuffer + sideChangeBuffer) {
                        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, grid.center.y - (grid.centeredDimensions.y / 2));
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
                        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, grid.center.y + (grid.centeredDimensions.y / 2));
                        playerCurrentSide = Orientation.FORWARD;
                    }
                    float smoothieAngle = (float)Orientation.LEFT + ((Mathf.Abs(zDeviation) / cornerSmoothingBuffer - sideChangeBuffer) * 45);
                    setPlayerFacingAngle(smoothieAngle);
                    //Debug.Log("From left to forward");
                } else if (zDeviation < 0) {
                    if (Mathf.Abs(zDeviation) > cornerSmoothingBuffer + sideChangeBuffer) {
                        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, grid.center.y - (grid.centeredDimensions.y / 2));
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

    float inRange(float x, float min, float max) {
        if (x < min) return x-min;
        if (x > max) return x-max;
        return 0;
    }

    void playerBlockChangeCheck() {
        playerCurrentBlock.x = Mathf.CeilToInt(gridBoundsMax.x - player.transform.position.x / grid.individualColumnDimension);
        playerCurrentBlock.y = Mathf.CeilToInt(gridBoundsMax.y - player.transform.position.z / grid.individualColumnDimension);
        //Debug.Log(playerCurrentBlock + "-" + playerCurrentSide);
    }

    public Orientation getPlayerCurrentSide() {
        return playerCurrentSide;
    }

    public Vector2Int getPlayerCurrentBlock() {
        return playerCurrentBlock;
    }

}

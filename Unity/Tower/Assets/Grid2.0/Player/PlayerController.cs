using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    protected const float shellRadius = 0.01f;
    protected const float minMoveDistance = 0.001f;
    protected Rigidbody rb;
    protected PlayerManager manager;
    protected Vector2 targetVelocity;
    protected Vector2 velocity2D;
    protected Vector3 velocity;
    protected Vector3 groundNormal = new Vector3(0f, 1f, 0f);
    protected RaycastHit[] hitBuffer = new RaycastHit[16];
    protected List<RaycastHit> hitBufferList = new List<RaycastHit>(16);
    protected Vector3 lastPosition;

    [Header("Death Settings")]
    public bool invincible = false;
    public LayerMask deathMask;
    [Range (0,1)]
    public float upwardsCollisionDetection = 0.5f;

    [Header("Physics Settings")]
    public LayerMask collisionMask;
    public float gravityModifier = 1f;
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    [HideInInspector]
    public bool isGrounded = false;
    [HideInInspector]
    public float facingAngle = 0;
    public float minGroundNormalY = 0.65f;
    [HideInInspector]
    public bool hasMoved = false;

    void Start(){
        rb = GetComponent<Rigidbody>();
        manager = GetComponent<PlayerManager>();
    }

    void Update(){ //Manages input
        targetVelocity = Vector2.zero;
        computeVelocity();
        //if(!invincible)deathCheck();
    }

    protected void computeVelocity() { //In 2D
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");    //Get x input
        if (Input.GetButtonDown("Jump") && isGrounded) { //get y input
            velocity2D.y = jumpTakeOffSpeed;
        } else if (Input.GetButtonUp("Jump")) {
            if (velocity2D.y > 0)
                velocity2D.y *= 0.5f;
        }
        targetVelocity = move * maxSpeed; //calculate target Velocity
    }

    void FixedUpdate() { //Process and call movement
        velocity2D += gravityModifier * new Vector2(Physics.gravity.x, Physics.gravity.y) * Time.deltaTime;  //Adding gravity
        velocity2D.x = targetVelocity.x;  //Adding input
        isGrounded = false;
        Vector2 deltaPosition = velocity2D * Time.deltaTime;
        Vector3 tempGroundNormal = Quaternion.AngleAxis(-facingAngle, Vector3.up) * groundNormal; //translating groundNormal to 2D
        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 move2D = moveAlongGround * deltaPosition.x;

        //Passing to rotated projection     in 3D
        velocity = rotate(new Vector3(velocity2D.x, velocity2D.y, 0f));
        Vector3 move = rotate(new Vector3(move2D.x, move2D.y, 0f));
        lastPosition = rb.position;
        movement(move, false);
        move = Vector2.up * deltaPosition.y;
        movement(move, true);
        hasMoved = rb.position != lastPosition;
    }

    void movement(Vector3 move, bool yMovement) {
        float distance = move.magnitude;
        if (distance > minMoveDistance) {
            hitBuffer = rb.SweepTestAll(move, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < hitBuffer.Length; i++) {
                if(collisionMask.value == 1 << hitBuffer[i].transform.gameObject.layer) hitBufferList.Add(hitBuffer[i]);   //get collisions
            }
            for (int i = 0; i < hitBufferList.Count; i++) {
                Vector3 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY) {  //check if collisions are grounds
                    isGrounded = true;
                    if (yMovement) {
                        groundNormal = currentNormal;
                        currentNormal.x = currentNormal.z = 0;
                    }
                }
                float projection = Vector3.Dot(velocity, currentNormal);
                if (projection < 0) {
                    velocity = velocity - projection * currentNormal;   //modify velocity
                }
                float modifiedDistance = hitBufferList[i].distance - shellRadius;   //modify distance
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        rb.position = rb.position + move.normalized * distance;  //move
    }

    Vector3 rotate(Vector3 vector) {
        return Quaternion.AngleAxis(facingAngle, Vector3.up) * vector;
    }

    public void setAngle(float physicsAngle, float graphicsAngle) {
        facingAngle = physicsAngle;
        transform.rotation = Quaternion.Euler(0, graphicsAngle + 180, 0);
    }

    protected void deathCheck() {
        bool dead = false;
        GameObject killer = null;
        hitBuffer = rb.SweepTestAll(transform.up, GetComponent<Collider>().bounds.extents.y + upwardsCollisionDetection);
        for (int i = 0; i < hitBuffer.Length && !dead; i++) {
            if (deathMask.value == 1 << hitBuffer[i].transform.gameObject.layer) {
                killer = hitBuffer[i].transform.gameObject;
                dead = true;
            }
        }
        if (dead) manager.kill(killer);
    }

    private void OnTriggerStay(Collider other) {
        bool dead = false;
        GameObject killer = null;
        if (deathMask.value == 1 << other.transform.gameObject.layer && !invincible) {
            killer = other.transform.gameObject;
            dead = true;
        }
        if (dead) manager.kill(killer);
    }
}

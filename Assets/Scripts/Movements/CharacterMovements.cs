using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovements : MonoBehaviour
{
    /* ==== User-defined data members ==== */
    [SerializeField] public float   airControlRatio     = .05f;
    [Range(0f, 1f)]
    [SerializeField] public float   wallFriction        = .5f;
    [SerializeField] public float   speedScale          = 3f;
    [SerializeField] public float   jumpSpeed           = 8f;
    [SerializeField] public float   gravity             = 20f;
    [SerializeField] public float   wallDetectionRange  = 1f;
    [SerializeField] public float   wallJumpNormalSpeed = 5f;
    [SerializeField] public float   wallJumpUpSpeed     = 5f;
    // This is not physically correct, but it gives a better video-game-like jump.
    [SerializeField] private float  fallAcceleration    = .1f;

    /* ==== Private data members ==== */
    public CharacterController      controller          = null;
    public Vector3                  moveDirection       = Vector3.zero;
    public float                    inputSpeed          = 0f;
    public float                    defaultZValue       = 0f;
    public bool                     isOnWall            = false;

    /* ==== Public data members ==== */
    internal bool                   disableInputs       = false;
    internal bool                   JumpFlag            { get; set; }
    internal bool                   WallJumpFlag        { get; set; }

    


    /* ==== Unity methods ==== */
    private void Awake()
    {
        JumpFlag    = false;
        controller  = GetComponent<CharacterController>();
    }


    private void Start()
    {
        defaultZValue = gameObject.transform.localPosition.z;
    }


    void Update()
    {
        if (controller.isGrounded)
        {
            disableInputs = false;
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(inputSpeed, 0.0f, 0f);
            moveDirection *= speedScale;

            // Try to jump
            if (JumpFlag)
            {
                moveDirection.y = jumpSpeed;
                JumpFlag        = false;
                WallJumpFlag    = false;
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            moveDirection.y -= gravity * Time.deltaTime;
            // Move the player.       
            controller.Move(moveDirection * Time.deltaTime);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, defaultZValue); // to lock Z axis, not lockable by rigid body constraints or any other methods.
        }

        else
        {
            // Move in mid-air with input
            if (!disableInputs)
                moveDirection.x = inputSpeed * speedScale * airControlRatio;

            //if (isOnWall)
            //    moveDirection.y += gravity / 2f * Time.deltaTime;

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            //moveDirection -= jumpLastVelocity;
            moveDirection.y -= gravity * Time.deltaTime;

            TryToWallJump(ref moveDirection);

            if (moveDirection.y < 0f)
                moveDirection.y -= fallAcceleration;

            if (isOnWall && moveDirection.y < 0f)
            {
                moveDirection.y *= wallFriction;
            }

            // Move the player.       
            controller.Move(moveDirection * Time.deltaTime);
            // to lock Z axis, not lockable by rigid body constraints or any other methods.
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, defaultZValue);
        }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // if collision with ceil, then set y velocity to 0
        if (moveDirection.y > 0f && hit.normal.y < -.3f)
            moveDirection.y = 0f;

        if ((moveDirection.x > 0f && hit.normal.x < -.3f) ||
            (moveDirection.x < 0f && hit.normal.x > .3f))
            moveDirection.x = 0f;
    }


    /* ==== Character's abilities ==== */
    public void MoveX(float f)
    {
        if (disableInputs)
            return;

        inputSpeed = f;
        if (.1f < f)
        {
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            //foreach (GameObject obj in models)
            //    obj.transform.rotation = Quaternion.Euler(0, 90f, 0);
        }

        else if (f < -.1f)
        {
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            //foreach (GameObject obj in models)
            //    obj.transform.rotation = Quaternion.Euler(0, -90f, 0);
        }
    }


    public void CopyFrom(CharacterMovements other)
    {
        JumpFlag        = other.JumpFlag;
        inputSpeed      = other.inputSpeed;
        isOnWall        = other.isOnWall;
        moveDirection   = other.moveDirection;
    }


    private void TryToWallJump(ref Vector3 velocity)
    {
        if (controller.isGrounded)
            return;

        // ======== Detect Wall ======== //
        Ray ray         = new Ray();
        ray.origin      = transform.position - Vector3.up * 0.5f;
        ray.direction   = transform.forward;
            
        RaycastHit hitInfo; 
        if (Physics.Raycast(ray, out hitInfo, wallDetectionRange))
        {
            disableInputs   = false;
            isOnWall        = true;
        }

        else
            isOnWall = false;

        // ======== If input, then jump ======== //
        if (isOnWall && WallJumpFlag && !controller.isGrounded)
        {
            velocity        = hitInfo.normal * wallJumpNormalSpeed + Vector3.up * wallJumpUpSpeed;
            disableInputs   = true;
            isOnWall = WallJumpFlag = false;

            // Rotate
            if (velocity.x > .1f)
                transform.rotation = Quaternion.Euler(0f, 90f, 0f);

            else if (velocity.x < -.1f)
                transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterMovementProperties
{
    [SerializeField] public float airControlRatio = .9f;
    [Range(0f, 1f)]
    [SerializeField] public float wallFriction = .5f;
    [SerializeField] public float speedScale = 3f;
    [SerializeField] public float jumpSpeed = 8f;
    [SerializeField] public float gravity = 20f;

    [SerializeField] public bool canWallJump = true;
    [SerializeField] public float wallDetectionRange = 1f;
    [SerializeField] public float wallJumpNormalSpeed = 5f;
    [SerializeField] public float wallJumpUpSpeed = 5f;
    [SerializeField] public float fallAcceleration = .1f;

    [SerializeField] public bool avoidSlowMotion = false;

    internal CharacterMovementProperties(bool bAvoidSlowMotion)
    {
        avoidSlowMotion = bAvoidSlowMotion;
    }

    internal void scaleMotion(float ratio)
    {
        speedScale           *= ratio;
        jumpSpeed            *= ratio;
        gravity              *= ratio * ratio;
        wallJumpNormalSpeed  *= ratio;
        wallJumpUpSpeed      *= ratio;
        fallAcceleration     *= ratio;
    }
}

[RequireComponent(typeof(CharacterController))]
public class CharacterMovements : MonoBehaviour
{
    /* ==== User-defined data members ==== */
    internal CharacterMovementProperties properties;

    //public float   AirControlRatio     {get; set;}
    //public float   WallFriction        {get; set;}
    //public float   SpeedScale          {get; set;}
    //public float   JumpSpeed           {get; set;}
    //public float   Gravity             {get; set;}

    //public  bool   CanWallJump         {get; set;}
    //public float   WallDetectionRange  {get; set;}
    //public float   WallJumpNormalSpeed {get; set;}
    //public float   WallJumpUpSpeed     {get; set;}

    //// This is not physically correct, but it gives a better video-game-like jump.
    //public float  FallAcceleration     {get; set;}

    /* ==== Private data members ==== */
    private float                    inputSpeed          = 0f;
    private float                    defaultZValue       = 0f;
    private bool                     isOnWall            = false;

    /* ==== Public data members ==== */
    internal CharacterController    controller          = null;
    internal bool                   disableInputs       = false;
    internal Vector3                moveDirection       = Vector3.zero;
    internal bool                   JumpFlag            { get; set; }
    internal bool                   WallJumpFlag        { get; set; }
    internal CharacterController    Controller          { get; }

    
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

    public void DirectMove(Vector3 motion)
    {
        if (controller != null)
        {
            controller.Move(motion);
        }    
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            disableInputs = false;
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(inputSpeed, 0.0f, 0f);
            moveDirection *= properties.speedScale;

            // Try to jump
            if (JumpFlag)
            {
                moveDirection.y = properties.jumpSpeed;
                JumpFlag        = false;
                WallJumpFlag    = false;
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            moveDirection.y -= properties.gravity * Time.deltaTime;
            // Move the player.       
            controller.Move(moveDirection * Time.deltaTime);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, defaultZValue); // to lock Z axis, not lockable by rigid body constraints or any other methods.
        }

        else
        {
            // Move in mid-air with input
            if (!disableInputs)
                moveDirection.x = inputSpeed * properties.speedScale * properties.airControlRatio;

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            moveDirection.y -= properties.gravity * Time.deltaTime;

            TryToWallJump(ref moveDirection);

            if (moveDirection.y < 0f)
                moveDirection.y -= properties.fallAcceleration;

            if (properties.canWallJump && isOnWall && moveDirection.y < 0f)
            {
                moveDirection.y *= properties.wallFriction;
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
        {
            inputSpeed = 0f;
            return;
        }

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
        if (!properties.canWallJump || controller.isGrounded)
            return;

        // ======== Detect Wall ======== //
        Ray ray         = new Ray();
        ray.origin      = transform.position - Vector3.up * 0.5f;
        ray.direction   = transform.forward;
            
        RaycastHit hitInfo; 
        if (Physics.Raycast(ray, out hitInfo, properties.wallDetectionRange))
        {
            disableInputs   = false;
            isOnWall        = true;
        }

        else
            isOnWall = false;

        // ======== If input, then jump ======== //
        if (isOnWall && WallJumpFlag && !controller.isGrounded)
        {
            velocity        = hitInfo.normal * properties.wallJumpNormalSpeed + Vector3.up * properties.wallJumpUpSpeed;
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
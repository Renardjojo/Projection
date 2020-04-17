using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioComponent
{
    public AudioClip walkingFootSound1 = null;
    internal AudioSource walkingFootSoundSourceAudio1;
    public AudioClip walkingFootSound2 = null;
    internal AudioSource walkingFootSoundSourceAudio2;

    internal bool isPlayerFirstSound = true;
    public float walkSoundDelay = 0.3f;
    public float walkSoundOffSet = 0f;
    internal float walkDelay = 0f;

    public AudioClip spawnSound = null;
    internal AudioSource spawnSourceAudio;

    public AudioClip deadSound = null;
    internal AudioSource deadSourceAudio;

    public AudioClip jumpStartSound = null;
    internal AudioSource jumpStartSourceAudio;

    /*public      AudioClip jumpEndSound = null;
     internal    AudioSource jumpEndSourceAudio;*/

    public AudioClip transposeSound = null;
    internal AudioSource transposeSourceAudio;

    public AudioClip interractSound = null;
    internal AudioSource interractSourceAudio;

    public AudioClip resetPositionSound = null;
    internal AudioSource resetPositionSourceAudio;
}

[System.Serializable]
public class CharacterMovementProperties
{
    [SerializeField] public float airControlRatio = .9f;
    [Range(0f, 1f)]
    [SerializeField] public float wallFriction = .5f;
    [SerializeField] public float speedScale = 3f;
    [SerializeField] public float jumpSpeed = 8f;
    [SerializeField] public float gravity = 20f;

    [SerializeField] public bool  canWallJump = true;
    [SerializeField] public float inputWallJumpSaveDelay = 0.2f;
    [SerializeField] public float wallDetectionRange = 1f;
    [SerializeField] public float wallJumpNormalSpeed = 5f;
    [SerializeField] public float wallJumpUpSpeed = 5f;
    [SerializeField] public float fallAcceleration = .1f;
    [SerializeField] public float airControlRatioWhenWallJump = 100f;
    [SerializeField] public float maxVelocity = 70f;
    [SerializeField] public float coyoteTime = 0.1f;


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
        maxVelocity          *= ratio;
    }
}

[RequireComponent(typeof(CharacterController))]
public class CharacterMovements : MonoBehaviour
{
    /* ==== User-defined data members ==== */
    internal CharacterMovementProperties    properties;
    internal AudioComponent                 audio;
    internal Animator                       animator;
    Coroutine                               wallJumpDelayCorroutine;

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

    // Corresponds to the last time the player was on ground.
    // If the player is currently on the ground, 
    // it means that this value is equal to Time.time .
    // It is used for the Coyote Time.
    private float lastGroundTime = 0f;
    private bool isCoyoteTimeAvailable = true; 


    /* ==== Unity methods ==== */
    private void Awake()
    {
        JumpFlag    = false;
        controller  = GetComponent<CharacterController>();
    }


    private void Start()
    {
        defaultZValue = gameObject.transform.localPosition.z;
        initializeSoundComponent();
    }


    void initializeSoundComponent()
    {
        if (audio.walkingFootSound1 != null)
        {
            audio.walkingFootSoundSourceAudio1 = gameObject.AddComponent<AudioSource>();
            audio.walkingFootSoundSourceAudio1.clip = audio.walkingFootSound1;

            audio.walkDelay = audio.walkSoundOffSet % audio.walkSoundDelay;
        }

        if (audio.walkingFootSound2 != null)
        {
            audio.walkingFootSoundSourceAudio2 = gameObject.AddComponent<AudioSource>();
            audio.walkingFootSoundSourceAudio2.clip = audio.walkingFootSound2;

            audio.walkDelay = audio.walkSoundOffSet % audio.walkSoundDelay;
        }

        if (audio.spawnSound != null)
        {
            audio.spawnSourceAudio = gameObject.AddComponent<AudioSource>();
            audio.spawnSourceAudio.clip = audio.spawnSound;
        }

        if (audio.deadSound != null)
        {
            audio.deadSourceAudio = gameObject.AddComponent<AudioSource>();
            audio.deadSourceAudio.clip = audio.deadSound;
        }

        if (audio.jumpStartSound != null)
        {
            audio.jumpStartSourceAudio = gameObject.AddComponent<AudioSource>();
            audio.jumpStartSourceAudio.clip = audio.jumpStartSound;
        }

        /*
        if (audio.jumpEndSound != null)
        {
            audio.jumpEndSourceAudio = gameObject.AddComponent<AudioSource>();
            audio.jumpEndSourceAudio.clip = audio.jumpEndSound;
        }*/

        if (audio.transposeSound != null)
        {
            audio.transposeSourceAudio = gameObject.AddComponent<AudioSource>();
            audio.transposeSourceAudio.clip = audio.transposeSound;
        }

        if (audio.interractSound != null)
        {
            audio.interractSourceAudio = gameObject.AddComponent<AudioSource>();
            audio.interractSourceAudio.clip = audio.interractSound;
        }

        if (audio.resetPositionSound != null)
        {
            audio.resetPositionSourceAudio = gameObject.AddComponent<AudioSource>();
            audio.resetPositionSourceAudio.clip = audio.resetPositionSound;
        }
    }

    public void DirectMove(Vector3 motion)
    {
        if (controller != null && controller.gameObject.activeSelf)
        {
            controller.Move(motion);
        }    
    }

    private void TryToJump()
    {            
        // Try to jump
        if (JumpFlag)
        {
            audio.jumpStartSourceAudio?.Play();

            moveDirection.y = properties.jumpSpeed;
            JumpFlag = false;
            WallJumpFlag = false;
            isCoyoteTimeAvailable = false;
        }
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            animator?.SetBool("IsGrounded", true);

            isCoyoteTimeAvailable = true;
            lastGroundTime = Time.time;

            disableInputs = false;
            // We are grounded, so recalculate
            // move direction directly from axes

            if (!disableInputs)
                moveDirection = new Vector3(inputSpeed, 0f, 0f);
            else
                moveDirection = Vector3.zero;

            moveDirection *= properties.speedScale;

            TryToJump();

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            moveDirection.y -= properties.gravity * Time.deltaTime;
            // Move the player.

            // ======== Detect MovingWall ======== //
            Ray ray = new Ray();
            ray.origin = transform.position;
            ray.direction = -transform.up * 10f;

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, properties.wallDetectionRange) && (hitInfo.collider.tag == "MovingWall"))
            {
                controller.Move((moveDirection * Time.deltaTime) + hitInfo.collider.GetComponent<MovingObject>().frameDisplacement);
            }
            else
            {
                controller.Move(moveDirection * Time.deltaTime);
            }


            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, defaultZValue); // to lock Z axis, not lockable by rigid body constraints or any other methods.
        }
        else
        {
            animator?.SetBool("IsGrounded", false);

            // Move in mid-air with input
            if (!disableInputs)
                moveDirection.x = inputSpeed * properties.speedScale * properties.airControlRatio;
            else
            {
                moveDirection.x += inputSpeed * properties.speedScale * properties.airControlRatio / properties.airControlRatioWhenWallJump;
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            moveDirection.y -= properties.gravity * Time.deltaTime;

            TryToJump();
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
        if (f != 0f && controller.isGrounded)
        {
            audio.walkDelay += Time.unscaledDeltaTime;

            if (audio.walkDelay >= (audio.walkSoundDelay * (Mathf.Abs(Mathf.Abs(f) - 1f) + 1f)))
            {
                audio.walkDelay = 0f;

                if (audio.isPlayerFirstSound)
                    audio.walkingFootSoundSourceAudio1?.Play();
                else
                    audio.walkingFootSoundSourceAudio2?.Play();

                audio.isPlayerFirstSound = !audio.isPlayerFirstSound;
            }
        }

        inputSpeed = f;

        if (disableInputs)
            return;

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
        if (Physics.Raycast(ray, out hitInfo, properties.wallDetectionRange) && (hitInfo.collider.tag == "Wall" || hitInfo.collider.tag == "MovingWall"))
        {
            disableInputs = false;
            isOnWall = true;
            animator?.SetBool("IsOnWall", true);
        }

        else
        {
            isOnWall = false;
            animator?.SetBool("IsOnWall", false);
        }

        // ======== If input, then jump ======== //
        if (isOnWall && WallJumpFlag && !controller.isGrounded)
        {
            velocity        = hitInfo.normal * properties.wallJumpNormalSpeed + Vector3.up * properties.wallJumpUpSpeed;
            disableInputs   = true;
            isOnWall = WallJumpFlag = false;
            animator?.SetBool("IsOnWall", false);
            animator?.SetTrigger("WallJump");

            // Rotate
            if (velocity.x > .1f)
                transform.rotation = Quaternion.Euler(0f, 90f, 0f);

            else if (velocity.x < -.1f)
                transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
    }

    bool TryToUseCoyoteTime()
    {
        if (isCoyoteTimeAvailable && Time.time - lastGroundTime < properties.coyoteTime)
        {
            isCoyoteTimeAvailable = false;
            return true;
        }
        else
            return false;
    }

    public void Jump()
    {                           
        if (controller.isGrounded || TryToUseCoyoteTime())
        {
            JumpFlag = true;
        }
        else
        {
            if (wallJumpDelayCorroutine != null)
                StopCoroutine(wallJumpDelayCorroutine);

            wallJumpDelayCorroutine = StartCoroutine(AllowWallJumpDelayCorroutine());
        }
    }

    IEnumerator AllowWallJumpDelayCorroutine()
    {
        float timer = 0f;
        WallJumpFlag = true;

        while (timer < properties.inputWallJumpSaveDelay && WallJumpFlag)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        WallJumpFlag = false;
    }
}
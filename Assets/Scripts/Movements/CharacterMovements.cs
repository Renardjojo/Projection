using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovements : MonoBehaviour
{
    private float inputSpeed = 0f;

    [SerializeField]
    private float dashMaxTime = 1f;
    private float dashCurrentTime = 0f;
    [SerializeField]
    private float dashCooldown = 0f;
    private float dashCooldownLeft = 0f;

    [SerializeField]
    private float airControlRatio = 0.05f;

    internal bool JumpFlag { get; set; }
    internal bool DashFlag { get; set; }

    internal bool preventInputsUntilGround = false;

    public void MoveX(float f)
    {
        inputSpeed = f;
        if (f > 0.1)
        {
            transform.rotation = Quaternion.Euler(0, 90f, 0);
            //foreach (GameObject obj in models)
            //    obj.transform.rotation = Quaternion.Euler(0, 90f, 0);
        }
        if (f < - 0.1)
        {
            transform.rotation = Quaternion.Euler(0, -90f, 0);
            //foreach (GameObject obj in models)
            //    obj.transform.rotation = Quaternion.Euler(0, -90f, 0);
        }
    }

    private CharacterController controller = null;

    [SerializeField]
    private float speedScale = 3.0f;
    [SerializeField]
    private float jumpSpeed = 8.0f;
    [SerializeField] private float gravity = 20.0f;

    // This is not physically correct, but it gives a better video game like jump.
    [SerializeField] private float accelerationWhenFalling = 0.1f;

    public Vector3 moveDirection = Vector3.zero;

    private float defaultZValue;

    public void CopyFrom(CharacterMovements other)
    {
        JumpFlag = other.JumpFlag;
        inputSpeed = other.inputSpeed;
        isOnWall = other.isOnWall;

        moveDirection = other.moveDirection;
    }

    private void Awake()
    {
        JumpFlag = false;
        DashFlag = false;

        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        defaultZValue = gameObject.transform.localPosition.z;
    }

    private void DashUpdate()
    {
        if (dashCurrentTime >= dashMaxTime)
        {
            dashCurrentTime = 0f;
            dashCooldownLeft = dashCooldown;
        }

        dashCooldownLeft -= Time.deltaTime;

        if (DashFlag && moveDirection.sqrMagnitude != 0f && dashCooldownLeft <= 0)
        {
            controller.Move(moveDirection.normalized * Time.deltaTime * 10);
            dashCurrentTime += Time.deltaTime;
            return;
        }
    }

    private Vector3 lastNormal;
    private bool isOnWall = false;

    private void TryToWallJump(ref Vector3 velocity)
    {
        Physics.Raycast(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        //if (isOnWall)
        //{
        //    velocity = lastNormal;
        //    velocity.y = 0.9f;
        //    velocity = velocity.normalized * speedScale * 3;
        //    isOnWall = false;
        //    preventInputsUntilGround = true;
        //}
    }

    void Update()
    {
        DashUpdate();

        if (controller.isGrounded)
        {
            preventInputsUntilGround = false;
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(inputSpeed, 0.0f, 0f);
            moveDirection *= speedScale;

            // Try to jump
            if (JumpFlag)
            {
                moveDirection.y = jumpSpeed;
                JumpFlag = false;
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
             moveDirection.x = inputSpeed * speedScale * airControlRatio;

            if (isOnWall)
                moveDirection.y += gravity / 2f * Time.deltaTime;

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            //moveDirection -= jumpLastVelocity;
            moveDirection.y -= gravity * Time.deltaTime;

            TryToWallJump(ref moveDirection);

            if (!preventInputsUntilGround && moveDirection.y < 0f)
                moveDirection.y -= accelerationWhenFalling;

            // Move the player.       
            controller.Move(moveDirection * Time.deltaTime);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, defaultZValue); // to lock Z axis, not lockable by rigid body constraints or any other methods.
        }
    }
}
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

    internal bool JumpFlag {get; set;}
    internal bool DashFlag { get; set; }

    public void MoveX(float f)
    {
        inputSpeed = f;
    }

    //private Rigidbody rb = null;
    private CharacterController controller = null;

    [SerializeField]
    private float speedScale = 3.0f;
    [SerializeField]
    private float jumpSpeed = 8.0f;
    [SerializeField] private float gravity = 20.0f;

    // This is not physically correct, but it gives a better video game like jump.
    [SerializeField] private float accelerationWhenFalling = 0.1f;

    private Vector3 moveDirection = Vector3.zero;

    private float defaultZValue;

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

    void Update()
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

        if (controller.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(inputSpeed, 0.0f, 0f);
            moveDirection *= speedScale;

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
            if (!Mathf.Approximately(inputSpeed, 0f))
                moveDirection.x = inputSpeed * speedScale * airControlRatio;

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            //moveDirection -= jumpLastVelocity;
            moveDirection.y -= gravity * Time.deltaTime;

            if (moveDirection.y < 0)
                moveDirection.y -= 0.1f;

            // Move the player.       
            controller.Move(moveDirection * Time.deltaTime);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, defaultZValue); // to lock Z axis, not lockable by rigid body constraints or any other methods.
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Moves cube
        if (hit.gameObject.tag == "Cube")
        {
            hit.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(hit.moveDirection.x * 10, 0f, 0f), ForceMode.Force);
        }
    }
}
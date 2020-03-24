using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
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

    private bool bIsJumping = false;

    public void MoveX(float f)
    {
        inputSpeed = f;
    }

    private Rigidbody rb = null;
    private CharacterController controller = null;

    [SerializeField]
    private float speedScale = 3.0f;
    [SerializeField]
    private float jumpSpeed = 8.0f;
    [SerializeField]
    private float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }


    internal void Jump()
    {
        bIsJumping = true;
    }


    void Update()
    {
        if (dashCurrentTime >= dashMaxTime)
        {
            dashCurrentTime = 0f;
            dashCooldownLeft = dashCooldown;
        }

        dashCooldownLeft -= Time.deltaTime;

        if (Input.GetKey(KeyCode.F) && moveDirection.sqrMagnitude != 0f && dashCooldownLeft <= 0)
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

            if (bIsJumping)
            {
                moveDirection.y = jumpSpeed;
                bIsJumping = false;
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            moveDirection.y -= gravity * Time.deltaTime;
            // Move the player.       
            controller.Move(moveDirection * Time.deltaTime);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -2.5f); // to lock Z axis, not lockable by rigid body constraints or any other methods.
        }
        else
        {
            Vector3 moveDirection2 = new Vector3(inputSpeed, 0.0f, 0f);
            moveDirection2 *= speedScale * airControlRatio;

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            //moveDirection -= jumpLastVelocity;
            moveDirection.y -= gravity * Time.deltaTime;

            // Move the player.       
            controller.Move((moveDirection + moveDirection2) * Time.deltaTime);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -2.5f); // to lock Z axis, not lockable by rigid body constraints or any other methods.
        }
    }
}
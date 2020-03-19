using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float jump = 10f;

    [SerializeField]
    private Rigidbody rb;

    private bool bJump = false;
    private bool isGrounded = true;

    private float horizontalMove = 0f; 

    // Start is called before the first frame update
    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveX(float value)
    {
        horizontalMove = value;
    }

    public void moveY(float value)
    {
        if (value > 0f)
            bJump = true;
        //transform.position += new Vector3(0, Time.deltaTime * value * speed, 0);
    }

    private void FixedUpdate()
    {
        if (isGrounded && bJump)
        {
            rb.velocity += new Vector3(0, jump, 0);
            isGrounded = false;
        }
        bJump = false;


        transform.position += new Vector3(Time.fixedDeltaTime * horizontalMove * speed, 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("ON GROUND");
        if (collision.gameObject.tag == "ground")
        {
            {
                float dot = Vector3.Dot(collision.contacts[0].normal, Vector3.up);
                if (dot > 0.5 || dot < -0.5)
                {
                    isGrounded = true;
                }
            }

            {
                // TODO : fix wall block
                float dot = Vector3.Dot(collision.contacts[0].normal, Vector3.right);
                if (dot > 0.5 || dot < -0.5)
                {
                    if (rb.velocity.x > 0)
                        rb.velocity = new Vector3(0, rb.velocity.y);
                }
            }
        }
    }

    public void onQPressed()
    {
        Debug.Log("pressed");
    }

    public void onQReleased()
    {
        Debug.Log("released");
    }
}

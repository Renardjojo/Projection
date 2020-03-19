using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField]
    private float jump = 10f;

    private Rigidbody rb = null;

    private bool bJump = false;
    private bool isGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void StartJump(float value)
    {
        if (value > 0f)
            bJump = true;
    }

    private void FixedUpdate()
    {
        if (isGrounded && bJump)
        {
            rb.velocity += new Vector3(0, jump, 0);
            isGrounded = false;
        }
        bJump = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
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
}

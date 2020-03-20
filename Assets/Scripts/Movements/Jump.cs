using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private float jump = 10f;
    [SerializeField] private float sensibility = 0.075f;

    private Rigidbody rb = null;

    private bool bJump = false;
    private bool IsGrounded { get { return nbGroundCollsions > 0; } }

    private uint nbGroundCollsions = 0;

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
        if (value > sensibility)
        {
            bJump = true;
        }
    }

    private void FixedUpdate()
    {
        if (IsGrounded && bJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, jump, 0);
            //IsGrounded = false;
        }
        bJump = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            {
                //float dot = Vector3.Dot(collision.contacts[0].normal, Vector3.up);
                //if (dot > 0.5 || dot < -0.5)
                //{
                    Debug.Log("Enter");
                    nbGroundCollsions++;
                    //isGrounded = true;
                //}
            }

            //{
            //    // TODO : fix wall block
            //    float dot = Vector3.Dot(collision.contacts[0].normal, Vector3.right);
            //    if (dot > 0.5 || dot < -0.5)
            //    {
            //        if (rb.velocity.x > 0)
            //            rb.velocity = new Vector3(0, rb.velocity.y);
            //    }
            //}
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            Debug.Log("Exit");
            nbGroundCollsions--;
            //isGrounded = true;

            //if (collision.contacts.Length > 0)
            //{
            //    // TODO : fix wall block
            //    float dot = Vector3.Dot(collision.contacts[0].normal, Vector3.right);
            //    if (dot > 0.5 || dot < -0.5)
            //    {
            //        if (rb.velocity.x > 0)
            //            rb.velocity = new Vector3(0, rb.velocity.y);
            //    }
            //}
        }
    }
}

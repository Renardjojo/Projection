using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private float jump = 10f;
    [SerializeField] private float sensibility = 0.075f;

    private Rigidbody rb = null;

    private bool bJump = false;
    private bool IsGrounded { get { return collidingObjects.Count > 0; } }

    private List<GameObject> collidingObjects = new List<GameObject>();

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
        }
        bJump = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            float dot = Vector3.Dot(collision.contacts[0].normal, Vector3.up);
            if (dot > 0.5)
            {
                collidingObjects.Add(collision.gameObject);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            collidingObjects.Remove(collision.gameObject);
        }
    }
}

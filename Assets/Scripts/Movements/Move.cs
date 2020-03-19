using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Move : MonoBehaviour
{
    [SerializeField]
    private float speed = 100f;

    private Rigidbody rb;

    private float horizontalMove = 0f; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveX(float value)
    {
       horizontalMove = value;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(Time.fixedDeltaTime * horizontalMove * speed, rb.velocity.y, 0);
    }

    public void OnQPressed()
    {
        Debug.Log("pressed");
    }

    public void OnQReleased()
    {
        Debug.Log("released");
    }
}

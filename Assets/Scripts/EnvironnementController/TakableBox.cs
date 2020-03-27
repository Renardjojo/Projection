using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakableBox : MonoBehaviour
{
    private GameObject owner = null;
    private Rigidbody rb = null;

    internal bool IsTaken { get { return owner == null; } }

    internal void TryToTakeBox(GameObject newOwner, float maxDistancce)
    {
        //Debug.Log("JEO");
        //if (Physics.Raycast(newOwner.transform.position, newOwner.transform.forward, maxDistancce))
        if ((newOwner.transform.position - transform.position).sqrMagnitude < 3*3)
        {
            Take(newOwner);                 
        }
    }

    public void Take(GameObject newOwner)
    {
        rb.useGravity  = false;
        rb.isKinematic = true;
        owner = newOwner;
    }

    public void Drop()
    {
        if (owner != null)
        {
            rb.useGravity  = true;
            rb.isKinematic = false;
            owner = null;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (owner != null)
        {
            transform.position = owner.transform.position + owner.transform.forward * 1f;
        }
    }
}

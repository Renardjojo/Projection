using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakableBox : MonoBehaviour
{
    [SerializeField]
    private float interactionRadius = 1.75f;

    private GameObject owner = null;
    private Rigidbody rb = null;

    internal bool IsTaken { get { return owner != null; } }

    internal void TryToTakeBox(GameObject newOwner, float maxDistancce)
    {
        //if (Physics.Raycast(newOwner.transform.position, newOwner.transform.forward, maxDistancce))
        if ((newOwner.transform.position - transform.position).sqrMagnitude < interactionRadius  * interactionRadius)
        {
            Take(newOwner);                 
        }
    }

    public void Take(GameObject newOwner)
    {
        rb.detectCollisions = false;
        rb.useGravity  = false;
        rb.isKinematic = true;
        owner = newOwner;
    }

    public void Drop()
    {
        if (owner != null)
        {
            rb.detectCollisions = true;
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

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

}

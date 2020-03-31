using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakableBox : MonoBehaviour
{
    [SerializeField] private float interactionRadius = 1.75f;

    private float cubeSize = 0.5f; // used for raycasting


    private GameObject owner = null;
    private Rigidbody rb = null;
    private Collider colliderBox = null;

    internal bool IsTaken { get { return owner != null; } }

    internal bool TryToTakeBox(GameObject newOwner, Collider takerCollider)
    {
        //if (Physics.Raycast(newOwner.transform.position, newOwner.transform.forward, maxDistancce))
        if ((newOwner.transform.position - transform.position).sqrMagnitude < interactionRadius  * interactionRadius)
        {
            Take(newOwner, takerCollider);
            return true;
        }
        return false;
    }

    private Collider playerCollider = null;
    public void Take(GameObject newOwner, Collider takerCollider)
    {
        Debug.Log("box  " + colliderBox);
        Debug.Log("take  " + takerCollider);
        Physics.IgnoreCollision(colliderBox, takerCollider, true);
        //rb.detectCollisions = false;
        rb.useGravity  = false;
        //rb.isKinematic = true;
        owner = newOwner;

        playerCollider = takerCollider;
    }

    public void Drop(Collider takerCollider)
    {
        if (owner != null)
        {
            Debug.Log("drop  " + takerCollider);
            Physics.IgnoreCollision(colliderBox, takerCollider, false);
            //rb.detectCollisions = true;
            rb.useGravity  = true;
            //rb.isKinematic = false;
            owner = null;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        colliderBox = GetComponent<Collider>();
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
            const int layerIndex = 2;
            const float maxDistance = 1f;

            int oldPlayerLayer = playerCollider.gameObject.layer;
            int oldBoxLayer = colliderBox.gameObject.layer;
            playerCollider.gameObject.layer = layerIndex;
            colliderBox.gameObject.layer = layerIndex;


            // get bit mask (example : 10000000)
            int layerMask = 1 << layerIndex;
            // Inverts the bitmask for raycast collision, (example : 01111111)
            layerMask = ~layerMask;

            RaycastHit hit;
            if (Physics.Raycast(owner.transform.position, owner.transform.forward, out hit, maxDistance, layerMask))
            {
                // Set location to hit point.
                // However, that would mean half the the cube would be inside the wall.
                // To prevent that, we move back by substracting the forward * cubeHalfSize.
                transform.position = hit.point - owner.transform.forward * cubeSize / 2f;
            }
            else
            {
                transform.position = owner.transform.position + owner.transform.forward * maxDistance;
            }

            playerCollider.gameObject.layer = oldPlayerLayer;
            colliderBox.gameObject.layer = oldBoxLayer;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

}

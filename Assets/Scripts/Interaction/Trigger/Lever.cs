using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Trigger
{
    [SerializeField] private float interactionRadius = 2f;

    private float interactionRadius2;

    private void Awake()
    {
        interactionRadius2 = interactionRadius * interactionRadius;
    }

    public void TryToSwitch(Vector3 playerPos)
    {
        if ((playerPos - transform.position).sqrMagnitude < interactionRadius2)
        {
            Toggle();
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }


    public void OnDrawGizmosSelected()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Trigger
{
    [SerializeField] private float interactionRadius = 2f;

    public void Switch()
    {
        if (IsOn)
        {
            IsOn = false;
            Disable();
        }
        else
        {
            IsOn = true;
            Enable();
        }
    }

    public void TryToSwitch(Vector3 playerPos)
    {
        if ((playerPos - transform.position).sqrMagnitude < interactionRadius * interactionRadius)
        {
            Switch();
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

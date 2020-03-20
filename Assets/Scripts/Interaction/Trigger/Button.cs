using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Trigger
{
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private float activationLength = 1f;

    IEnumerator ReleaseCoroutine()
    {
        // wait for activationLength seconds
        yield return new WaitForSeconds(activationLength);

        // Disable
        IsOn = false;
        Disable();
    }

    public void Press()
    {
        IsOn = true;
        Enable();
        StopCoroutine(ReleaseCoroutine());
        StartCoroutine(ReleaseCoroutine());
    }

    public void TryToPress(Vector3 playerPos)
    {
        if ((playerPos - transform.position).sqrMagnitude < interactionRadius * interactionRadius)
        {
            Press();
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}

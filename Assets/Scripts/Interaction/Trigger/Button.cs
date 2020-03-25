﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Trigger
{
    [SerializeField] private float interactionRadius = 2f;
    [SerializeField] private float activationLength = 1f;

    private Coroutine releaseCoroutine = null;
    private float interactionRadius2;

    private void Start()
    {
        interactionRadius2 = interactionRadius * interactionRadius;
    }

    IEnumerator ReleaseCoroutine()
    {
        // wait for activationLength seconds
        yield return new WaitForSeconds(activationLength);

        Disable();
    }

    public void Press()
    {
        Enable();

        if (releaseCoroutine != null)
            StopCoroutine(releaseCoroutine);

        releaseCoroutine = StartCoroutine(ReleaseCoroutine());
    }

    public void TryToPress(Vector3 playerPos)
    {
        if ((playerPos - transform.position).sqrMagnitude < interactionRadius2)
        {
            Press();
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Trigger
{
    [Header("Range within the player can press the button")]
    [SerializeField] private float interactionRadius = 2f;
    // When the pressed, the button will be on. 
    // After waiting activationLength seconds, the button will be off again.
    [Header("When the pressed, the button will be on. After waiting activationLength seconds, the button will be off again.")]
    [SerializeField] private float activationLength = 1f;

    private Coroutine releaseCoroutine = null;
    private float interactionRadius2;

    private void Awake()
    {
        interactionRadius2 = interactionRadius * interactionRadius;
    }

    IEnumerator ReleaseCoroutine()
    {
        // wait for activationLength seconds
        yield return new WaitForSeconds(activationLength);

        IsOn = false;
    }

    public void Press()
    {
        IsOn = true;

        if (releaseCoroutine != null)
            StopCoroutine(releaseCoroutine);

        releaseCoroutine = StartCoroutine(ReleaseCoroutine());
    }

    public void Release()
    {
        if (releaseCoroutine != null)
            StopCoroutine(releaseCoroutine);

        IsOn = false;
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

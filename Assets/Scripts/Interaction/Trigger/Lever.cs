using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Trigger
{
    [SerializeField] private GameObject baseSphere = null;
    [SerializeField, Range(1f, 80f)] private float leverAngle = 20f;
    [SerializeField, Range(0f, 5f)] private float leverLerpWait = 2f;

    [Header("Range within the player can press the button"), SerializeField] private float interactionRadius = 2f;

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
            StopAllCoroutines();
            StartCoroutine(RotateLever(IsOn));
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }


    public void OnDrawGizmosSelected()
    {
        
    }

    IEnumerator RotateLever(bool movement)
    {
        float elapsedTime = 0;

        while (elapsedTime < leverLerpWait)
        {
            baseSphere.transform.localRotation = Quaternion.Lerp(baseSphere.transform.localRotation, Quaternion.Euler(new Vector3(0, 0, movement ? leverAngle : -leverAngle)), (elapsedTime / leverLerpWait));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}

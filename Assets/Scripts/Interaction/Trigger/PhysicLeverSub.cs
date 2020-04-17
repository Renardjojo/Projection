using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PhysicLeverSub : MonoBehaviour
{
    [SerializeField] private PhysicLever lever = null;
    [SerializeField] private GameObject baseSphere = null;
    [SerializeField, Range(1f, 80f)] private float leverAngle = 20f;
    [SerializeField, Range(0f, 5f)] private float leverLerpWait = 2f;

    private void OnTriggerEnter(Collider other)
    {
        CharacterController charController = other.gameObject.GetComponent<CharacterController>();
        if (charController)
        {
            float dot = 2 * Vector3.Dot(charController.velocity, transform.right /* normal of the lever direction, so normal of transform.up, and since we are in 2D, it is the transform.up = transform.right */);

            //transform.localRotation = Quaternion.Euler(new Vector3(0, 0, dot < 0 ? 15 : -15));

            lever.IsOn = (dot < 0);
            StopAllCoroutines();
            StartCoroutine(RotateLever(lever.IsOn));
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    // if axisRotationID is 0, the platform will rotate around the x axis
    // if it is 1, around the y axis
    // if it is 2, around the z axis
    [SerializeField, Range(0,2), Tooltip("0 = rotate around x axis\n1 = rotate around y axis\n2 = rotate around z axis")] int axisRotationID = 2;
    [SerializeField] float maxRotation = 180f;

    private Vector3 defaultRotation;
    private float lastTime = 0f;
    private float targetRotation = 0f;
    private float lastRotation   = 0f;

    bool bIsRotating = false;
    bool bIsFaceType1 = false;

    [SerializeField]
    private float duration = 3f;

    private void Awake()
    {
        defaultRotation = transform.rotation.eulerAngles;
    }

    public void Activate()
    {
        if (!bIsRotating)
        {
            lastRotation = targetRotation;
            bIsFaceType1 = !bIsFaceType1;
            if (bIsFaceType1)
                targetRotation = maxRotation;
            else
                targetRotation = 0;

            lastTime = Time.time;
            bIsRotating = true;
        }
    }

    private void FixedUpdate()
    {
        if (bIsRotating)
        {
            Vector3 newRot = defaultRotation;

            float delta = (Time.time - lastTime) / (duration);

            // Be aware of rotation bugs caused by Gimbal lock
            newRot[axisRotationID] = Mathf.LerpAngle(lastRotation, targetRotation, delta);

            transform.eulerAngles = newRot;

            if (delta >= 1f)
                bIsRotating = false;
        }
    }
}

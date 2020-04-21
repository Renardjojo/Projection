using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    // if axisRotationID is 0, the platform will rotate around the x axis
    // if it is 1, around the y axis
    // if it is 2, around the z axis
    [Tooltip("0 = rotate around x axis\n1 = rotate around y axis\n2 = rotate around z axis")]
    [Range(0, 2)]
    [SerializeField] int axisRotationID = 2;

    [SerializeField] float maxRotation = 180f;

    private float   lastTime;
    private float   originalRotation;
    private float   targetRotation;

    private bool bIsRotating;
    private bool bIsFaceType1;

    [SerializeField]
    private float duration = 3f;

    private void Awake()
    {
        originalRotation = transform.rotation.eulerAngles[axisRotationID];
        lastTime = targetRotation = 0f;
        bIsRotating = bIsFaceType1 = false;
    }

    public void Activate()
    {
        bIsFaceType1 = !bIsFaceType1;
        if (bIsFaceType1)
            targetRotation = originalRotation + maxRotation;
        else
            targetRotation = originalRotation;

        lastTime = Time.time;
        bIsRotating = true;
    }


    private void FixedUpdate()
    {
        if (bIsRotating)
        {
            Vector3 newRot = transform.rotation.eulerAngles;

            float delta = (Time.time - lastTime) / (duration);

            // Be aware of rotation bugs caused by Gimbal lock
            newRot[axisRotationID] = Mathf.LerpAngle(newRot[axisRotationID], targetRotation, delta);

            transform.eulerAngles = newRot;

            if (delta >= 1f)
                bIsRotating = false;
        }
    }
}

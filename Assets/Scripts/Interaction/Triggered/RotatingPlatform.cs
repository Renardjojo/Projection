using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{

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
                targetRotation = 180;
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

            // Be aware of rotation bugs caused by 
            newRot.x = Mathf.LerpAngle(lastRotation, targetRotation, delta);

            transform.eulerAngles = newRot;

            if (delta >= 1f)
                bIsRotating = false;
        }
    }
}

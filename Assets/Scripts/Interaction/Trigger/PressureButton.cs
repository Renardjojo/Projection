using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PressureButton : Trigger
{
    [SerializeField] private uint necessaryCollidingObjects = 1;
    private uint currentCollidingObjects = 0;


    // Start is called before the first frame update
    void Start()
    {
        IsOn = false;
    }

    //  Returns false if we should ignore the collision.
    //  Else, returns true;
    private bool IsInputCollision(Collision collision)
    {
        return collision.gameObject.CompareTag("BodyPlayer");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsInputCollision(collision))
        {
            currentCollidingObjects++;
            UpdateButton();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsInputCollision(collision))
        {
            currentCollidingObjects--;
            UpdateButton();
        }
    }

    private void UpdateButton()
    {
        if (IsOn && currentCollidingObjects < necessaryCollidingObjects)
        {
            OnDisable();
        }
        else if (!IsOn && currentCollidingObjects >= necessaryCollidingObjects)
        {
            Enable();
        }
    }
}
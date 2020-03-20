using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PressureButton : Trigger
{
    [SerializeField] private uint necessaryPressure = 1;
    private uint currentPressure = 0;


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
            currentPressure++;
            UpdateButton();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsInputCollision(collision))
        {
            currentPressure--;
            UpdateButton();
        }
    }

    private void UpdateButton()
    {
        if (IsOn && currentPressure < necessaryPressure)
        {
            OnDisable();
        }
        else if (!IsOn && currentPressure >= necessaryPressure)
        {
            Enable();
        }
    }
}
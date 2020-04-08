using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PressureButton : Trigger
{
    [SerializeField] private String[] tagsWithCollisionEnabled = new string[] { "BodyPlayer" };
    [SerializeField] private uint necessaryCollidingObjects = 1;
    private uint currentCollidingObjects = 0;

    //  Returns false if we should ignore the collision.
    //  Else, returns true
    private bool IsInputCollision(Collider collision)
    {
        foreach (String tag in tagsWithCollisionEnabled)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsInputCollision(Collision collision)
    {
        foreach (String tag in tagsWithCollisionEnabled)
        {
            if (collision.gameObject.CompareTag(tag))
            {
                return true;
            }
        }

        return false;
    }

    internal void OnTriggerEnter(Collider collision)
    {
        if (IsInputCollision(collision))
        {
            currentCollidingObjects++;
            UpdateButton();
        }
    }

    internal void OnTriggerExit(Collider collision)
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
            IsOn = false;

        else if (!IsOn && currentCollidingObjects >= necessaryCollidingObjects)
            IsOn = true;
    }
}
using UnityEngine;
using System;

public class PressureButton : Trigger
{
    [Header("Triggering parameters")]
    [Tooltip("Objects with the following tags will be able to use this button")]
    [SerializeField] private String[] tagsWithCollisionEnabled = new string[] { "BodyPlayer" };
    [Tooltip("Number of objects which must be on the button to trigger it")]
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
        if (!applyDelay)
        {
            if (IsOn && currentCollidingObjects < necessaryCollidingObjects)
                IsOn = false;

            else if (!IsOn && currentCollidingObjects >= necessaryCollidingObjects)
                IsOn = true;
        }
    }
}
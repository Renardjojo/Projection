using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PressureButton : Trigger
{
    [SerializeField] private String[] tagsWithCollisionEnabled = new string[] { "BodyPlayer" };
    [SerializeField] private uint necessaryCollidingObjects = 1;
    [Range(0f, 10f)]
    [Tooltip("Duration the pressure button will remain active, after stepping of it (in seconds)")]
    [SerializeField] private float automaticTimer = 0f;

    private bool    inCountdown;
    private uint    currentCollidingObjects = 0;
    private float   timeElapsed;

    private void Awake()
    {
        isOn        = false;
        timeElapsed = 0f;
        inCountdown = false;
    }


    private void Update()
    {
        if (inCountdown)
            UpdateCountdown();
    }

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
        if (isOn && currentCollidingObjects < necessaryCollidingObjects)
            inCountdown = true;

        else if (!isOn && currentCollidingObjects >= necessaryCollidingObjects)
        {
            Enable();
            Debug.Log("Activated");
        }
    }


    private void UpdateCountdown()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= automaticTimer)
        {
            Debug.Log("Deactivated");
            timeElapsed = 0f;
            inCountdown = false;
            Disable();
        }
    }
}
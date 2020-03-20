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

    private void OnCollisionEnter(Collision collision)
    {
        currentPressure++;
    }

    private void OnCollisionExit(Collision collision)
    {
        currentPressure--;
    }

    private void Update()
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
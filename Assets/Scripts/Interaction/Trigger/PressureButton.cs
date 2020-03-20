using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PressureButton : MonoBehaviour, Trigger
{
    [SerializeField] private uint necessaryPressure = 1;
    private uint currentPressure = 0;

    public bool IsOn { get; set; }

    public event Action onTriggered;
    public event Action onUntriggered;

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
            onUntriggered();
            IsOn = false;
        }
        else if (!IsOn && currentPressure >= necessaryPressure)
        {
            onTriggered();
            IsOn = true;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public abstract class Trigger : MonoBehaviour
{
    [SerializeField]
    public bool IsOn { get; set; }

    public event Action OnTriggered;
    public event Action OnUntriggered;

    protected void Enable()
    {
        IsOn = true;
        OnTriggered();
    }

    protected void OnDisable()
    {
        IsOn = false;
        OnUntriggered();
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public abstract class Trigger : MonoBehaviour
{
    [SerializeField]
    public bool IsOn { get; set; }

    public event Action OnTriggered   = null;
    public event Action OnUntriggered = null;

    protected void Enable()
    {
        IsOn = true;
        OnTriggered?.Invoke();
    }

    protected void Disable()
    {
        IsOn = false;
        OnUntriggered?.Invoke();
    }
}



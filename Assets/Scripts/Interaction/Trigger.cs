using UnityEngine;
using System;

public abstract class Trigger : MonoBehaviour
{
    [SerializeField]
    public bool isOn { get; set; }

    public event Action OnTriggered   = null;
    public event Action OnUntriggered = null;

    internal void Enable()
    {
        isOn = true;
        OnTriggered?.Invoke();
    }

    internal void Disable()
    {
        isOn = false;
        OnUntriggered?.Invoke();
    }

    internal void Toggle()
    {
        isOn = !isOn;
        if (isOn)   OnTriggered?.Invoke();
        else        OnUntriggered?.Invoke();
    }
}



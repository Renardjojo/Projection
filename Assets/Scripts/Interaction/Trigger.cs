using UnityEngine;
using System;

public abstract class Trigger : MonoBehaviour
{
    [SerializeField]
    public bool isOn { get; set; }

    public event Action OnTriggered   = null;
    public event Action OnUntriggered = null;

    protected void Enable()
    {
        isOn = true;
        OnTriggered?.Invoke();
    }

    protected void Disable()
    {
        isOn = false;
        OnUntriggered?.Invoke();
    }

    protected void Toggle()
    {
        isOn = !isOn;
        if (isOn)   OnTriggered?.Invoke();
        else        OnUntriggered?.Invoke();
    }
}



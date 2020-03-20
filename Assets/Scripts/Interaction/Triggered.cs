using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;

class Triggered : MonoBehaviour
{
    [SerializeField] protected List<Trigger>    triggerList      = null;
    [SerializeField] protected UnityEvent       OnActivatedEvent = null;
    [SerializeField] protected UnityEvent       OnDisabledEvent  = null;
                     protected bool             isActivate       = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Trigger trig in triggerList)
        {
            trig.OnTriggered   += TryToActivate;
            trig.OnUntriggered += TryToDeactivate;
        }
    }

    public void TryToActivate()
    {
        foreach (Trigger trig in triggerList)
        {
            if (!trig.IsOn)
                return;
        }

        OnActivated();
    }

    public void TryToDeactivate()
    {
        foreach (Trigger trig in triggerList)
        {
            if (!trig.IsOn)
            {
                OnDisabled();
                return;
            }
        }
    }

    public void OnActivated()
    {
        OnActivatedEvent?.Invoke();
    }

    public void OnDisabled()
    {
        OnDisabledEvent?.Invoke();
    }
}
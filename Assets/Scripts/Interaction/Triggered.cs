using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;

class Triggered : MonoBehaviour
{
    [SerializeField] protected List<Trigger>    triggerList;
    [SerializeField] protected UnityEvent       onActivatedEvent;
    [SerializeField] protected UnityEvent       onDisabledEvent;
    [SerializeField] protected bool             isActivate = false;

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
        onActivatedEvent?.Invoke();
    }

    public void OnDisabled()
    {
        onDisabledEvent?.Invoke();
    }
}
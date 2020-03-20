using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;

class Triggered : MonoBehaviour
{
    [SerializeField] protected List<Trigger>    TriggerList { get; set; }
    [SerializeField] protected UnityEvent       OnActivatedEvent;
    [SerializeField] protected UnityEvent       OnDisabledEvent;
    [SerializeField] protected bool             isActivate = false;

    public void TryToActivate()
    {
        foreach (Trigger trig in TriggerList)
        {
            if (!trig.IsOn)
                return;
        }

        OnActivated();
    }

    public void TryToDesactivate()
    {
        foreach (Trigger trig in TriggerList)
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
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class AndTriggerList
{
    public List<Trigger> andList;
}

class Triggered : MonoBehaviour
{
    [SerializeField] protected List<AndTriggerList> orTriggerList       = null;
    [SerializeField] protected UnityEvent           OnActivatedEvent    = null;
    [SerializeField] protected UnityEvent           OnDisabledEvent     = null;
                     protected bool                 isActivate          = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (AndTriggerList andTriggerList in orTriggerList)
        {
            foreach (Trigger trig in andTriggerList.andList)
            {
                trig.OnTriggered += TryToActivate;
                trig.OnUntriggered += TryToDeactivate;
            }
        }
    }

    public void TryToActivate()
    {
        bool Or = false;

        foreach (AndTriggerList andTriggerList in orTriggerList)
        {
            bool And = true;
            foreach (Trigger trig in andTriggerList.andList)
            {
                And &= trig.isOn;
            }
            Or |= And;
        }

        if (Or)
        {
            OnActivated();
        }
    }

    public void TryToDeactivate()
    {
        bool Or = false;

        foreach (AndTriggerList andTriggerList in orTriggerList)
        {
            bool And = true;
            foreach (Trigger trig in andTriggerList.andList)
            {
                And &= trig.isOn;
            }
            Or |= And;
        }

        if (!Or)
        {
            OnDisabled();
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
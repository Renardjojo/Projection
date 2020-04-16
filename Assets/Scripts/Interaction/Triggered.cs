using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class AndTriggerList
{
    public List<Trigger> andList;
}

public class Triggered : SoundPlayer
{
    [SerializeField] protected List<AndTriggerList> orTriggerList = null;
    [SerializeField] protected UnityEvent OnActivatedEvent = null;
    [SerializeField] protected UnityEvent OnDisabledEvent = null;
    protected bool isActivate = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (AndTriggerList andTriggerList in orTriggerList)
        {
            foreach (Trigger trig in andTriggerList.andList)
            {
                if (trig)
                {
                    trig.OnTriggered += TryToActivate;
                    trig.OnUntriggered += TryToDeactivate;
                }
            }
        }
    }

    public void TryToActivate()
    {
        bool Or = false;

        foreach (AndTriggerList andTriggerList in orTriggerList)
        {
            if (andTriggerList.andList.Count == 0)
                continue;

            bool And = true;

            foreach (Trigger trig in andTriggerList.andList)
            {
                if (trig)
                    And &= trig.IsOn;
            }
            Or |= And;
        }

        if (Or)
            OnActivated();
    }

    public void TryToDeactivate()
    {
        bool Or = false;

        foreach (AndTriggerList andTriggerList in orTriggerList)
        {
            if (andTriggerList.andList.Count == 0)
                continue;

            bool And = true;

            foreach (Trigger trig in andTriggerList.andList)
            {
                if (trig)
                    And &= trig.IsOn;
            }
            Or |= And;
        }

        if (!Or)
            OnDisabled();
    }

    public void OnActivated()
    {
        isActivate = true;
        OnActivatedEvent?.Invoke();
        PlaySound();
    }

    public void OnDisabled()
    {
        isActivate = false;
        OnDisabledEvent?.Invoke();
        PlaySound();
    }

    protected override void PlaySound()
    {
        if (isActivate || useSameSound)
            switchedOnAudio?.Play();
        else
            switchedOffAudio?.Play();
    }
}
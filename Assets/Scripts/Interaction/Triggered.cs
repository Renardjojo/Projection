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
    [SerializeField] protected bool singleUse = false;
    [SerializeField] protected List<AndTriggerList> orTriggerList = null;
    [SerializeField] protected UnityEvent OnActivatedEvent = null;
    [SerializeField] protected UnityEvent OnDisabledEvent = null;
    protected bool isActivate = false;

    private void Awake()
    {
        if (switchedOnSound)
        {
            switchedOnAudio         = gameObject.AddComponent<AudioSource>();
            switchedOnAudio.clip    = switchedOnSound;
            switchedOnAudio.volume  = volume;
        }

        if (useSameSound)
            switchedOffAudio = switchedOnAudio;

        else if (switchedOffSound)
        {
            switchedOffAudio        = gameObject.AddComponent<AudioSource>();
            switchedOffAudio.clip   = switchedOffSound;
            switchedOffAudio.volume = volume;
        }
    }

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
        if (isActivate)
            return;

        bool Or = false;

        foreach (AndTriggerList andTriggerList in orTriggerList)
        {
            // There is a list, but it is empty, or it contains a single null element. Don't evaluate it
            if (andTriggerList.andList.Count == 0 ||
                (andTriggerList.andList.Count == 1 && !andTriggerList.andList[0]))
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
        if (!isActivate || (isActivate && singleUse))
            return;

        bool Or = false;

        foreach (AndTriggerList andTriggerList in orTriggerList)
        {
            // There is a list, but it is empty, or there is a single null element. Don't evaluate it
            if (andTriggerList.andList.Count == 0 ||
                (andTriggerList.andList.Count == 1 && !andTriggerList.andList[0]))
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
        if (singleUse && isActivate)
            return;

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
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class AndTriggerList
{
    public List<Trigger> andList;
}

public class Triggered : MonoBehaviour
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
                GameDebug.AssertInTransform(trig != null, gameObject.transform, "Trigger should not be null");

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
                And &= trig.IsOn;
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
                And &= trig.IsOn;
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

/*
using UnityEngine;
using System;

public abstract class Trigger : MonoBehaviour
{
[Tooltip("Initial state of the trigger")]
[SerializeField] public bool isOn;

[Header("Manual trigger")]
[Range(0f, 10f)]
[Tooltip("Duration the pressure button will remain active, after stepping off it (seconds)")]
[SerializeField] protected float deactivationDelay = 0f;

[Header("Periodic trigger")]
[Tooltip("If this is toggled, the trigger will automatically switch between on and off")]
[SerializeField] protected bool automaticToggle = false;
[Tooltip("Time the trigger will remain \"on\" after being activated (seconds)")]
[SerializeField] [Range(0f, 10f)] protected float onDuration = 0f;
[Tooltip("Time the trigger will remain \"off\" after being disabled (seconds)")]
[SerializeField] [Range(0f, 10f)] protected float offDuration = 0f;


protected bool countdownStarted = false;
protected float timeElapsed = 0f;

public event Action OnTriggered = null;
public event Action OnUntriggered = null;



private void Update()
{
    if (automaticToggle)
    {
        timeElapsed += Time.deltaTime * Time.timeScale;
        if (isOn)
        {
            if (timeElapsed >= onDuration)
            {
                timeElapsed -= onDuration;
                isOn = false;
                OnUntriggered?.Invoke();
            }
        }

        else
        {
            if (timeElapsed >= offDuration)
            {
                timeElapsed -= offDuration;
                isOn = true;
                OnTriggered?.Invoke();
            }
        }
    }

    else if (countdownStarted && !isOn)
    {
        timeElapsed += Time.deltaTime * Time.timeScale;
        if (timeElapsed >= deactivationDelay)
        {
            timeElapsed = 0f;
            countdownStarted = false;
            OnUntriggered?.Invoke();
            Debug.Log("OnUntriggered.Invoke()");
        }
    }
}

internal void Enable()
{
    Debug.Log("Turned on");
    isOn = true;
    OnTriggered?.Invoke();
    countdownStarted = false;
}

internal void Disable()
{
    Debug.Log("Turned off");
    isOn = false;
    timeElapsed = 0f;
    countdownStarted = true;
}

internal void Toggle()
{
    isOn = !isOn;
    if (isOn) OnTriggered?.Invoke();
    else OnUntriggered?.Invoke();
}
}

*/

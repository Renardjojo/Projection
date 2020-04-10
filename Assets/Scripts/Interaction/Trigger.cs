using UnityEngine;
using System;

public abstract class Trigger : SoundPlayer
{
    /* ==== User accessible members ==== */
    [Header("Initial state")]
    [SerializeField]
    private bool isOn;

    [Header("Delays")]
    [Tooltip("Time after which this will react, once this is set \"on\"(seconds)")]
    [Range(0f, 10f), SerializeField]
    protected float activationDelay = 0f;

    [Tooltip("Time after which this will react, once this is set \"off\"(seconds)")]
    [Range(0f, 10f), SerializeField]
    protected float deactivationDelay = 0f;


    /* ==== Private data members ==== */
    protected float timeElapsed;
    private bool applyDelay;


    /* ==== Property ==== */
    public bool IsOn
    {
        get
        { return isOn; }

        set
        {
            if (isOn != value)
            {
                isOn        = value;
                applyDelay  = true;
                PlaySound();
            }
        }
    }


    /* ==== Actions ==== */
    public event Action OnTriggered = null;
    public event Action OnUntriggered = null;


    /* ==== Methods ==== */
    private void Awake()
    {
        timeElapsed = 0f;
        applyDelay = false;

        if (switchedOnSound)
        {
            switchedOnAudio = gameObject.AddComponent<AudioSource>();
            switchedOnAudio.clip = switchedOnSound;
        }

        if (!useSameSound && switchedOffSound)
        {
            switchedOffAudio = gameObject.AddComponent<AudioSource>();
            switchedOffAudio.clip = switchedOffSound;
        }
    }


    private void Start()
    {
        if (isOn)   OnTriggered?.Invoke();
        else        OnUntriggered?.Invoke();
    }


    protected void Update()
    {
        if (applyDelay)
        {
            timeElapsed += Time.deltaTime;

            if (isOn && timeElapsed >= activationDelay)
            {
                OnTriggered?.Invoke();
                timeElapsed = 0f;
                applyDelay = false;
            }

            else if (timeElapsed >= deactivationDelay)
            {
                OnUntriggered?.Invoke();
                timeElapsed = 0f;
                applyDelay = false;
            }
        }
    }


    protected override void PlaySound()
    {
        if (isOn || useSameSound)
            switchedOnAudio?.Play();
        else 
            switchedOffAudio?.Play();
    }


    public void Toggle()
    {
        isOn = !isOn;
        applyDelay = true;
    }
}
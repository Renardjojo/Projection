using UnityEngine;
using System;

public abstract class Trigger : SoundPlayer
{
    [Tooltip("A sound played during the transition from on to off")]
    [SerializeField]
    protected AudioClip offToOnSound = null;

    [Tooltip("A sound played during the transition from on to off")]
    [SerializeField]
    protected AudioClip onToOffSound = null;

    [Tooltip("The factor applied to the audio when time is slowed")]
    [Range(.01f, 1f)]
    [SerializeField]
    protected float audioSlowFactor = .5f;

    [Tooltip("Volume of the transition audio clips")]
    [Range(0f, 1f)]
    [SerializeField]
    protected float transitionVolume = 1f;

    protected AudioSource offToOnAudio;
    protected AudioSource onToOffAudio;

    /* ==== User accessible members ==== */
    [Header("Initial state")]
    [SerializeField] private bool isOn;
    [SerializeField] private bool isOnNeutralPositionAtStart = false;

    [Header("Delays")]
    [Tooltip("Time after which this will react, once this is set \"on\"(seconds)")]
    [Range(0f, 10f), SerializeField]
    protected float activationDelay = 0f;

    [Tooltip("Time after which this will react, once this is set \"off\"(seconds)")]
    [Range(0f, 10f), SerializeField]
    protected float deactivationDelay = 0f;


    /* ==== Private data members ==== */
    protected float timeElapsed;
    protected bool applyDelay;


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
                offToOnAudio?.Stop();
                onToOffAudio?.Stop();
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

        // SoundPlayer
        if (switchedOnSound)
        {
            switchedOnAudio         = gameObject.AddComponent<AudioSource>();
            switchedOnAudio.clip    = switchedOnSound;
            switchedOnAudio.volume  = volume;
        }


        if (offToOnSound)
        {
            offToOnAudio        = gameObject.AddComponent<AudioSource>();
            offToOnAudio.clip   = offToOnSound;
            offToOnAudio.loop   = true;
            offToOnAudio.volume = transitionVolume;
        }

        if (useSameSound)
        {
            switchedOffAudio    = switchedOnAudio;
            onToOffAudio        = offToOnAudio;
        }

        else if (switchedOffSound)
        {
            switchedOffAudio        = gameObject.AddComponent<AudioSource>();
            switchedOffAudio.clip   = switchedOffSound;
            switchedOffAudio.volume = volume;
        }

        if (onToOffSound)
        {
            onToOffAudio        = gameObject.AddComponent<AudioSource>();
            onToOffAudio.clip   = onToOffSound;
            onToOffAudio.loop   = true;
            onToOffAudio.volume = transitionVolume;
        }
    }


    protected void Update()
    {
        if (Time.timeScale < 1f)
        {
            if (switchedOnAudio)    switchedOnAudio.pitch   = audioSlowFactor;
            if (switchedOffAudio)   switchedOffAudio.pitch  = audioSlowFactor;
            if (offToOnAudio)       offToOnAudio.pitch      = audioSlowFactor;
            if (onToOffAudio)       onToOffAudio.pitch      = audioSlowFactor;
        }

        else
        {
            if (switchedOnAudio)    switchedOnAudio.pitch   = 1f;
            if (switchedOffAudio)   switchedOffAudio.pitch  = 1f;
            if (offToOnAudio)       offToOnAudio.pitch      = 1f;
            if (onToOffAudio)       onToOffAudio.pitch      = 1f;
        }

        if (applyDelay)
        {
            timeElapsed += Time.deltaTime;

            if (isOn)
            {
                if (timeElapsed >= activationDelay)
                {
                    OnTriggered?.Invoke();
                    offToOnAudio?.Stop();
                    timeElapsed = 0f;
                    applyDelay = false;
                }

                else if (offToOnAudio && !offToOnAudio.isPlaying)
                    offToOnAudio?.Play();
            }

            else if (timeElapsed >= deactivationDelay)
            {
                OnUntriggered?.Invoke();
                onToOffAudio?.Stop();
                timeElapsed = 0f;
                applyDelay = false;
            }

            else if (onToOffAudio && !onToOffAudio.isPlaying)
                onToOffAudio?.Play();
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
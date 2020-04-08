using UnityEngine;
using System;

public abstract class Trigger : MonoBehaviour
{
    /* ==== User accessible members ==== */
    [Tooltip("Initial state")]
    [SerializeField] private bool isOn;

    [Header("Delays")]
    [Tooltip("Time after which this will react, once this is set \"on\"(seconds)")]
    [Range(0f, 10f), SerializeField]
    protected float activationDelay = 0f;

    [Tooltip("Time after which this will react, once this is set \"off\"(seconds)")]
    [Range(0f, 10f), SerializeField]
    protected float deactivationDelay = 0f;

    public bool IsOn
    {
        get
        { return isOn; }

        set
        { isOn = value; applyDelay = true; }
    }


    /* ==== Protected data members ==== */

    protected bool applyDelay = false;
    protected float timeElapsed = 0f;


    /* ==== Actions ==== */
    public event Action OnTriggered   = null;
    public event Action OnUntriggered = null;


    /* ==== Methods ==== */
    private void Update()
    {
        if (applyDelay)
        {
            timeElapsed += Time.deltaTime * Time.timeScale;

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


    public void Toggle()
    {
        isOn = !isOn;
        applyDelay = true;
    }
}



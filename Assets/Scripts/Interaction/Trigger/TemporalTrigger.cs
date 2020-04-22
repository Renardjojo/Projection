using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalTrigger : Trigger
{
    [Tooltip("Time difference before this object starts switching on and off")]
    [SerializeField, Range(0f, 60f)] private float timeOffSet = 0f;
    [Tooltip("Time this object will remain on (s)")]
    [SerializeField, Range(0f, 60f)] private float OnDuration  = 2f;
    [Tooltip("Time this object will remain off (s)")]
    [SerializeField, Range(0f, 60f)] private float OffDuration = 1f;

    [SerializeField] private bool affectedByTimeSlow = true;

    private float timeElapsedForTempoTrigger = 0f;

    private void Start()
    {
        timeElapsedForTempoTrigger = timeOffSet % (OffDuration + OnDuration);
    }

    private new void Update()
    {
        timeElapsedForTempoTrigger += affectedByTimeSlow ? Time.deltaTime * Time.timeScale : Time.unscaledDeltaTime;

        if (IsOn)
        {
            if (timeElapsedForTempoTrigger >= OnDuration)
            {
                timeElapsedForTempoTrigger -= OnDuration;
                IsOn = false;
            }
        }

        else
        {
            if (timeElapsedForTempoTrigger >= OffDuration)
            {
                timeElapsedForTempoTrigger -= OffDuration;
                IsOn = true;
            }
        }

        base.Update();
    }
}
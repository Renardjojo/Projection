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

    private float timeElapsedForTempoTrigger = 0f;

    private void Start()
    {
        timeElapsedForTempoTrigger = timeOffSet % (OffDuration + OnDuration);
        timeElapsedForTempoTrigger %= (IsOn ? OffDuration : OnDuration);
    }

    private void Update()
    {
        timeElapsedForTempoTrigger += Time.deltaTime * Time.timeScale;

        if (IsOn)
        {
            if (timeElapsedForTempoTrigger >= OffDuration)
            {
                timeElapsedForTempoTrigger -= OffDuration;
                IsOn = false;
            }
        }

        else
        {
            if (timeElapsedForTempoTrigger >= OnDuration)
            {
                timeElapsedForTempoTrigger -= OnDuration;
                IsOn = true;
            }
        }

        base.Update();
    }
}
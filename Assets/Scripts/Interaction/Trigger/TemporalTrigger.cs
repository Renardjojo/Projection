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

    private void Start()
    {
        timeElapsed = timeOffSet % (IsOn ? OnDuration : OffDuration);
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime * Time.timeScale;

        if (IsOn)
        {
            if (timeElapsed >= OnDuration)
            {
                timeElapsed -= OnDuration;
                IsOn = false;
            }
        }

        else
        {
            if (timeElapsed >= OffDuration)
            {
                timeElapsed -= OffDuration;
                IsOn = true;
            }
        }

        base.Update();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalTrigger : Trigger
{
    [SerializeField, Range(0f, 60f)] private float timeOffSet = 0f;
    [SerializeField, Range(0f, 60f)] private float OnDuration  = 2f;
    [SerializeField, Range(0f, 60f)] private float OffDuration = 1f;

    float currentTimer = 0f;
    [SerializeField] bool isActivate = false;

    private void Start()
    {
        if (isActivate)
        {
            currentTimer = timeOffSet % OnDuration;
            Enable();
        }
        else
        {
            currentTimer = timeOffSet % OffDuration;
            Disable();
        }
    }

    private void Update()
    {
        currentTimer += Time.deltaTime;

        if (isActivate)
        {
            if (currentTimer >= OnDuration)
            {
                currentTimer -= OnDuration;
                isActivate = false;
                Disable();
            }
        }
        else
        {
            if (currentTimer >= OffDuration)
            {
                currentTimer -= OffDuration;
                isActivate = true;
                Enable();
            }
        }
    }
}

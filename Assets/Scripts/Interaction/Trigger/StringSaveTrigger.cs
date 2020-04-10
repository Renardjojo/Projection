using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringSaveTrigger : Trigger
{
    [SerializeField] string stringOfSaveToControl;

    private void OnEnable()
    {
        if (!IsOn && PlayerPrefs.GetInt(stringOfSaveToControl) == 1)
        {
            IsOn = true;
        }
        else if (IsOn && PlayerPrefs.GetInt(stringOfSaveToControl) != 1)
        {
            IsOn = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Dropdown))]
public class QualityControls : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<UnityEngine.UI.Dropdown>().onValueChanged.AddListener(ChangeInputs);
    }

    void ChangeInputs(int value)
    {
        QualitySettings.SetQualityLevel(value * 2, true);
    }
}

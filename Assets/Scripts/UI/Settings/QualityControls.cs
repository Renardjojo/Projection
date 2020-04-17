using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Dropdown))]
public class QualityControls : MonoBehaviour
{
    private UnityEngine.UI.Dropdown dropDown = null;

    // Start is called before the first frame update
    void Awake()
    {
        dropDown = GetComponent<UnityEngine.UI.Dropdown>();
        dropDown.value = QualitySettings.GetQualityLevel();

        dropDown.onValueChanged.AddListener(ChangeInputs);
    }

    void ChangeInputs(int value)
    {
        QualitySettings.SetQualityLevel(value * 2, true);
    }
}

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

        int qualityIndex = PlayerPrefs.GetInt("quality", 1);
        dropDown.value = qualityIndex;

        dropDown.onValueChanged.AddListener(ChangeInputs);
    }

    private void ChangeInputs(int value)
    {
        PlayerPrefs.SetInt("quality", value);
        QualitySettings.SetQualityLevel(value * 2, true);
    }
}

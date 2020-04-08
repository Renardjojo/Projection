using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityControls : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<UnityEngine.UI.Dropdown>().onValueChanged.AddListener(ChangeInputs);
    }

    void ChangeInputs(int value)
    {
        switch (value)
        {
            case 0:
                // Low
                break;
            case 1:
                // Medium
                break;
            case 2:
                // High
                break;
        }
    }
}

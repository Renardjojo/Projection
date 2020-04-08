using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsControl : MonoBehaviour
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
                // XBox
                break;
            case 1:
                // Azerty       
                break;
            case 2:
                // Qwerty
                break;
        }
    }
}

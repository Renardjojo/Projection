using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                PlayerPrefs.SetInt("inputs", 0b0000_0000_0000_0100);
                Debug.Log("xbox : " + 0x00000000000000000000000000000100b);
                // XBox
                break;
            case 1:
                PlayerPrefs.SetInt("inputs", 0b0000_0000_0000_0011);
                // Azerty       
                break;
            case 2:
                PlayerPrefs.SetInt("inputs", 0b0000_0000_0000_0010);
                // Qwerty
                break;
            case 3:
                // Default : gamepad + qwerty
                PlayerPrefs.SetInt("inputs", 0b0000_0000_0000_0110);
                break;
        }
    }
}

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
        PlayerPrefs.SetInt("inputs", value);
    }
}

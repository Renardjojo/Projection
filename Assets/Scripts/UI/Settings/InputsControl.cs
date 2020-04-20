using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UnityEngine.UI.Dropdown))]
public class InputsControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        UnityEngine.UI.Dropdown dropDown = GetComponent<UnityEngine.UI.Dropdown>();

        int inputIndex = PlayerPrefs.GetInt("inputs", 0);
        dropDown.value = inputIndex;

        dropDown.onValueChanged.AddListener(ChangeInputs);
    }

    private void ChangeInputs(int value)
    {
        PlayerPrefs.SetInt("inputs", value);
    }
}

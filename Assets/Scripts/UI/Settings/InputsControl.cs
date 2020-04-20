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
        dropDown.onValueChanged.AddListener(ChangeInputs);

        int n = PlayerPrefs.GetInt("inputs");
        dropDown.value = n;
    }

    void ChangeInputs(int value)
    {
        PlayerPrefs.SetInt("inputs", value);
    }
}

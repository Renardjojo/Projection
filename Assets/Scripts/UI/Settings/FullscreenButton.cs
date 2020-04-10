using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenButton : MonoBehaviour
{
    private void Awake()
    {
        Toggle toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(ChangeFullScreen);
    }

    private void ChangeFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}

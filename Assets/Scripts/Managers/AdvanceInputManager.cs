using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
/*
// Callbacks with no arguments.
[System.Serializable]
public class InputCallbacksV2
{
    public InputType type;
    //public string name;
    public UnityEvent onPressed, onReleased;

    public bool isOn;
}

[System.Serializable]
public class UnityEventFloatV2 : UnityEvent<float> { }

// Callbacks with no arguments.
[System.Serializable]
public class InputCallbacksFV2
{
    public InputType type;
    //public string name;
    public UnityEventFloat callback;
}*/
/*
[SerializeField, Tooltip("Calls \"onPressed\" and \"onReleased\" events when the key is pressed or released.")]
private List<InputCallbacks> digitalButtonlist = null;

[SerializeField, Tooltip("Calls \"callback\" each tick with different values depending on the axes.")]
private List<InputCallbacksF> analogButtonList = null;
*/

public enum ControlToUse
{
    Default,
    Control1,
    Control2,
    Control3
}

[System.Serializable]
public class Control
{
    [SerializeField] public string name;

    [SerializeField, Tooltip("Calls \"onPressed\" and \"onReleased\" events when the key is pressed or released.")]
    private List<InputCallbacks> digitalButtonlist;

    [SerializeField, Tooltip("Calls \"callback\" each tick with different values depending on the axes.")]
    private List<InputCallbacksF> analogButtonList;
}

public class AdvanceInputManager : MonoBehaviour
{
    [Header("Keyboard")]
    [SerializeField] private ControlToUse currentKeyboardControl;
    [SerializeField] private List<Control> keyboardControls;

    [Header("Game pad")]
    [SerializeField] private ControlToUse currentGamePadControl;
    [SerializeField] private List<Control> gamePadControls;
    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // For each button
        foreach (InputCallbacksV2 input in digitalButtonlist)
        {
            // Call callback if input is on
            if (Input.GetButton(input.type.ToString()))
            {
                if (!input.isOn)
                {
                    input.isOn = true;
                    input.onPressed?.Invoke();
                }
            }

            else if (input.isOn)
            {
                input.onReleased?.Invoke();
                input.isOn = false;
            }
        }

        // For each joystick
        foreach (InputCallbacksFV2 input in analogButtonList)
        {
            // Call callback with joystick values
            input.callback?.Invoke(Input.GetAxis(input.type.ToString()));
        }
    }*/
}

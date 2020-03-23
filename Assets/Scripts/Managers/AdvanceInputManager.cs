using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public enum ControlToUse : int
{
    Default = 0,
    Control1,
    Control2,
    Control3
}

public enum InputAnalogueType
{
    LeftHorizontal,
    LeftVertical,
    RightHorizontal,
    RightVertical
}

[System.Serializable]
public class UnityEventCallBackFloat : UnityEvent<float> { }

// Callbacks with no arguments.
[System.Serializable]
public class AdvanceDigitalInputCallbacks
{
    public KeyCode type;
    public UnityEvent onPressed, onReleased;
    public bool isOn;
}

[System.Serializable]
public class AdvanceAnalogicInputCallbacks
{
    public InputAnalogueType        type;
    public UnityEventCallBackFloat  callback;
    public float                    value;
}



[System.Serializable]
public class Control
{
    [SerializeField] public string name;

    [Tooltip("Calls \"onPressed\" and \"onReleased\" events when the key is pressed or released.")]
    public List<AdvanceDigitalInputCallbacks> digitalButtonlist;

    [Tooltip("Calls \"callback\" each tick with different values depending on the axes.")]
    public List<AdvanceAnalogicInputCallbacks> analogButtonList;
}

public class AdvanceInputManager : MonoBehaviour
{
    [Header("Keyboard")]
    [SerializeField] private ControlToUse currentKeyboardControl;
    [SerializeField] private List<Control> keyboardControls;

    [Header("Game pad")]
    [SerializeField] private ControlToUse currentGamePadControl;
    [SerializeField] private List<Control> gamePadControls;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // For each button
        foreach (AdvanceDigitalInputCallbacks input in keyboardControls[(int)currentKeyboardControl].digitalButtonlist)
        {
            // Call callback if input is on
            if (Input.GetKey(input.type))
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
        foreach (AdvanceAnalogicInputCallbacks input in keyboardControls[(int)currentKeyboardControl].analogButtonList)
        {
            // Call callback with joystick values
            input.callback?.Invoke(Input.GetAxis(input.type.ToString()));
        }
    }
}

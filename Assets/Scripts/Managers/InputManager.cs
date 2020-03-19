using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

// Callbacks with no arguments.
[System.Serializable]
public class InputCallbacks
{
    public InputType type;
    //public string name;
    public UnityEvent eventCallbacks;
}

[System.Serializable]
public class UnityEventFloat : UnityEvent<float> { }

// Callbacks with no arguments.
[System.Serializable]
public class InputCallbacksF
{
    public InputType type;
    //public string name;
    public UnityEventFloat eventCallbacks;
}


[System.Serializable]
public enum InputType
{
    Horizontal,
    Vertical
}

public enum ActionType
{
    LeaveGame,
    MoveX,
    MoveY,
    Jump,
}


public class InputManager : MonoBehaviour
{
    [SerializeField] private InputCallbacks[]  buttonList   = null;
    [SerializeField] private InputCallbacksF[] joystickList = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // For each button
        foreach (InputCallbacks input in buttonList)
        {
            // Call callback if input is on
            if (Input.GetButton(input.type.ToString()))
            {
                input.eventCallbacks?.Invoke();
            }
        }

        // For each joystick
        foreach (InputCallbacksF input in joystickList)
        {
            // Call callback with joystick values
            input.eventCallbacks?.Invoke(Input.GetAxis(input.type.ToString()));
        }
    }
}

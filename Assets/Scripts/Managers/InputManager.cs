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
    public UnityEvent onPressed, onReleased;

    public bool isOn;
}

[System.Serializable]
public class UnityEventFloat : UnityEvent<float> { }

// Callbacks with no arguments.
[System.Serializable]
public class InputCallbacksF
{
    public InputType type;
    //public string name;
    public UnityEventFloat callback;
}


[System.Serializable]
public enum InputType
{
    Horizontal,
    Vertical,
    Transpose,
    Interact,
    Jump
}


public class InputManager : MonoBehaviour
{
    [SerializeField, Tooltip("Calls \"onPressed\" and \"onReleased\" events when the key is pressed or released.")] 
    private List<InputCallbacks>  digitalButtonlist   = null;

    [SerializeField, Tooltip("Calls \"callback\" each tick with different values depending on the axes.")] 
    private List<InputCallbacksF> analogButtonList = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // For each button
        foreach (InputCallbacks input in digitalButtonlist)
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
        foreach (InputCallbacksF input in analogButtonList)
        {
            // Call callback with joystick values
            input.callback?.Invoke(Input.GetAxis(input.type.ToString()));
        }
    }
}

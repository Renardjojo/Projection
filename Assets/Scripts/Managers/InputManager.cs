using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum KeyboardCommand : byte
{
    MoveLeft = 0,
    MoveRight,
    Jump,
    Transpose,
    Interact,
    ResetShadow
}

public enum GamepadCommand : byte
{
    Move = 0,
    Jump,
    Transpose,
    Interact,
    ResetShadow
}


public enum GamepadCode : byte
{
    A = 0,
    B,
    X,
    Y,
    Start,
    LeftBumper,
    RightBumper,
    LeftHorizontal,
    LeftVertical,
    RightHorizontal,
    RightVertical,
    LeftTrigger,
    RightTrigger
}


[Serializable]
public class CommandKeyCodeDict : SerializableDictionary<KeyboardCommand, KeyCode>
{}


[Serializable]
public class CommandGamepadCodeDict : SerializableDictionary<GamepadCommand, GamepadCode>
{}


public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerController       controlledPlayer    = null;
    [SerializeField] private CinemachineVirtualCamera CMCam             = null;
    [SerializeField] private bool                   useKeyboard         = true;
    [SerializeField] private bool                   useGamepad          = true;
    [SerializeField] private CommandKeyCodeDict     keyboardControls    = null;
    [SerializeField] private CommandGamepadCodeDict gamepadControls     = null;
    [SerializeField] private bool                   useInspectorValues  = false;
    
    private Dictionary<KeyCode, KeyboardCommand>    keyboardInputs      = null;
    private Dictionary<string, GamepadCommand>      gamepadButtonInputs = null;
    private Dictionary<string, GamepadCommand>      gamepadAxisInputs   = null;

    // Prevent a single action from being executed twice (once wiht keyboard, once with gamepad)
    private bool hasMoved;
    private bool hasTransposed;
    private bool hasInteracted;

    // const masks
    private static int isGamepadMask = 0b0000_0000_0000_0100;
    private static int isKeyboardMask = 0b0000_0000_0000_0010;
    private static int isAzertyKeyboardMask = 0b0000_0000_0000_0001;

    private int currentInputOptionsData = 0b0000_0000_0000_0000; // gamepad

    private void Awake()
    {
        if (!controlledPlayer)
        {
            controlledPlayer = GameObject.FindObjectOfType<PlayerController>();

            if (!controlledPlayer)
            {
                useKeyboard = useGamepad = false;
                Debug.Log("No player was passed");
            }
        }

        // Loads inspector values
        if (useGamepad)
        {
            currentInputOptionsData |= isGamepadMask;
        }
        if (useKeyboard)
        {
            currentInputOptionsData |= isKeyboardMask;
        }

        LoadInputs(currentInputOptionsData);
    }

    public void LoadInputs(int inputOptionsData)
    {
        useGamepad  =  (inputOptionsData & isGamepadMask) != 0;
        useKeyboard = (inputOptionsData & isKeyboardMask) != 0;

        if (useGamepad) 
        {
            CommandGamepadCodeDict lgamepadControls = new CommandGamepadCodeDict();
            lgamepadControls.Add(GamepadCommand.Move, GamepadCode.LeftHorizontal);
            lgamepadControls.Add(GamepadCommand.Jump, GamepadCode.A);
            lgamepadControls.Add(GamepadCommand.Transpose, GamepadCode.LeftBumper);
            lgamepadControls.Add(GamepadCommand.Interact, GamepadCode.X);
            lgamepadControls.Add(GamepadCommand.ResetShadow, GamepadCode.RightBumper);

            //// Initialize gamepad inputs
            var inputs = ConvertToGamepadInputs(lgamepadControls);
            gamepadButtonInputs = inputs.gamepadButtonInputs;
            gamepadAxisInputs = inputs.gamepadAxisInputs;
        }
        
        if (useKeyboard) // 
        {
            CommandKeyCodeDict lkeyboardControls = new CommandKeyCodeDict();
            lkeyboardControls.Add(KeyboardCommand.Interact, KeyCode.E);
            lkeyboardControls.Add(KeyboardCommand.ResetShadow, KeyCode.R);
            lkeyboardControls.Add(KeyboardCommand.Transpose, KeyCode.F);
            lkeyboardControls.Add(KeyboardCommand.Jump, KeyCode.Space);

            // We already know the input is a keyboard, so we don't have to compute inputOptionsData & isKeyboardMask
            if ((inputOptionsData & isAzertyKeyboardMask) != 0 /* hex */) // is azerty
            {
                lkeyboardControls.Add(KeyboardCommand.MoveLeft, KeyCode.Q);
                lkeyboardControls.Add(KeyboardCommand.MoveRight, KeyCode.D);

                // set azerty
                keyboardInputs = ConvertToKeyboardInputs(lkeyboardControls);
            }
            else // is qwerty
            {
                lkeyboardControls.Add(KeyboardCommand.MoveLeft, KeyCode.A);
                lkeyboardControls.Add(KeyboardCommand.MoveRight, KeyCode.D);

                // set qwerty
                keyboardInputs = ConvertToKeyboardInputs(lkeyboardControls);
            }
        }
    }

    public static (Dictionary<string, GamepadCommand> gamepadButtonInputs,           
                   Dictionary<string, GamepadCommand> gamepadAxisInputs)    // Return a tuple
                   ConvertToGamepadInputs(CommandGamepadCodeDict lGamepadControls)
    {
        Dictionary<string, GamepadCommand> lGamepadButtonInputs = new Dictionary<string, GamepadCommand>();
        Dictionary<string, GamepadCommand> lGamepadAxisInputs = new Dictionary<string, GamepadCommand>();

        foreach (KeyValuePair<GamepadCommand, GamepadCode> kvp in lGamepadControls)
        {
            // Button
            if (kvp.Value < GamepadCode.LeftHorizontal)
            {
                lGamepadButtonInputs.Add(kvp.Value.ToString("g"), kvp.Key);
            }

            // Axis
            else
                lGamepadAxisInputs.Add(kvp.Value.ToString("g"), kvp.Key);
        }

        return (lGamepadButtonInputs, lGamepadAxisInputs);
    }

    public static Dictionary<KeyCode, KeyboardCommand> ConvertToKeyboardInputs(CommandKeyCodeDict lKeyboardControls)
    {
        Dictionary<KeyCode, KeyboardCommand> lKeyboardInputs = new Dictionary<KeyCode, KeyboardCommand>();
        foreach (KeyValuePair<KeyboardCommand, KeyCode> kpv in lKeyboardControls)
            lKeyboardInputs.Add(kpv.Value, kpv.Key);

        return lKeyboardInputs;
    }

    private void Update()
    {
        if (!useInspectorValues)
        {
            int tempInputOptions = PlayerPrefs.GetInt("inputs");
            if (tempInputOptions != currentInputOptionsData)
            {
                if (useKeyboard) // is keyboard
                {
                    keyboardControls.Clear();
                    keyboardInputs.Clear();
                }
                if (useGamepad) // is keyboard
                {
                    gamepadControls.Clear();
                    gamepadButtonInputs.Clear();
                    gamepadAxisInputs.Clear();
                }

                currentInputOptionsData = tempInputOptions;
                LoadInputs(currentInputOptionsData);
            }
        }

        hasMoved = hasTransposed = hasInteracted = false;

        if (useKeyboard)
        {
            // Listen to keyboard keys first
            foreach (KeyValuePair<KeyCode, KeyboardCommand> kvp in keyboardInputs)
            {
                if      (Input.GetKeyDown(kvp.Key)) HandleKeyDownCommand(kvp.Value);
                else if (Input.GetKey(kvp.Key))     HandleKeyPressedCommand(kvp.Value);
                else if (Input.GetKeyUp(kvp.Key))   HandleKeyUpCommand(kvp.Value);
            }
        }
        
        
        if (useGamepad)
        {
            // Listen to gamepad buttons
            foreach (KeyValuePair<string, GamepadCommand> kvp in gamepadButtonInputs)
            {
                if      (Input.GetButtonDown(kvp.Key))
                    HandleButtonDownCommand(kvp.Value);

                else if (Input.GetButton(kvp.Key))
                    HandleButtonPressedCommand(kvp.Value);

                else if (Input.GetButtonUp(kvp.Key))
                    HandleButtonUpCommand(kvp.Value);
            }

            // Listen to gamepad axis
            foreach (KeyValuePair<string, GamepadCommand> kvp in gamepadAxisInputs)
            {
                if (kvp.Key.Contains("Trigger"))
                    HandleTriggerCommand(kvp.Value, kvp.Key, Input.GetAxis(kvp.Key));
                else
                    HandleAxisCommand(kvp.Value, Input.GetAxis(kvp.Key));
            }
        }

        if (hasTransposed && CMCam)
            CMCam.Follow = controlledPlayer.controlledObject.transform;
    }


    // One of the gamepad button input is released
    private void HandleKeyDownCommand(KeyboardCommand command)
    {
        switch (command)
        {
            case KeyboardCommand.MoveLeft:
                break;
            
            case KeyboardCommand.MoveRight:
                break;

            case KeyboardCommand.Jump:
                controlledPlayer.Jump();
                break;

            case KeyboardCommand.Transpose:
                controlledPlayer.Transpose();
                hasTransposed = true;
                break;

            case KeyboardCommand.Interact:
                controlledPlayer.Interact();
                hasInteracted = true;
                break;

            case KeyboardCommand.ResetShadow:
                controlledPlayer.ResetShadow();
                break;
        }
    }


    // One of the keyboard input is pressed
    private void HandleKeyPressedCommand(KeyboardCommand command)
    {
        switch (command)
        {
            case KeyboardCommand.MoveLeft:
                controlledPlayer.MoveX(-1f);
                hasMoved = true;
                break;

            case KeyboardCommand.MoveRight:
                controlledPlayer.MoveX(1f);
                hasMoved = true;
                break;

            case KeyboardCommand.Jump:
                break;

            case KeyboardCommand.Transpose:
                break;

            case KeyboardCommand.Interact:
                break;

            case KeyboardCommand.ResetShadow:
                break;
        }
    }


    // One of the keyboard input is released
    private void HandleKeyUpCommand(KeyboardCommand command)
    {
        switch (command)
        {
            case KeyboardCommand.MoveLeft:
            case KeyboardCommand.MoveRight:
                controlledPlayer.MoveX(0f);
                hasMoved = true;
                break;

            case KeyboardCommand.Jump:
                controlledPlayer.Jump(false);
                break;

            case KeyboardCommand.Transpose:
                break;

            case KeyboardCommand.Interact:
                break;
        }
    }


    // One of the gamepad button input was pressed
    private void HandleButtonDownCommand(GamepadCommand command)
    {
        switch (command)
        {
            case GamepadCommand.Move:
                break;

            case GamepadCommand.Jump:
                controlledPlayer.Jump();
                break;

            case GamepadCommand.Transpose:
                if (!hasTransposed)
                {
                    controlledPlayer.Transpose();
                    hasTransposed = true;
                }
                break;

            case GamepadCommand.Interact:
                if (!hasInteracted)
                    controlledPlayer.Interact();
                break;

            case GamepadCommand.ResetShadow:
                controlledPlayer.ResetShadow();
                break;
        }
    }


    // One of the gamepad button input is being pressed
    private void HandleButtonPressedCommand(GamepadCommand command)
    {
        switch (command)
        {
            case GamepadCommand.Move:
                if (!hasMoved)
                    controlledPlayer.MoveX(1f);
                break;

            case GamepadCommand.Jump:
                break;

            case GamepadCommand.Transpose:
                break;

            case GamepadCommand.Interact:
                break;
        }
    }


    // One of the gamepad button input was released
    private void HandleButtonUpCommand(GamepadCommand command)
    {
        switch (command)
        {
            case GamepadCommand.Move:
                if (!hasMoved)
                    controlledPlayer.MoveX(0f);
                break;

            case GamepadCommand.Jump:
                controlledPlayer.Jump(false);
                break;

            case GamepadCommand.Transpose:
                break;

            case GamepadCommand.Interact:
                break;
        }
    }


    // Check Axis values
    private void HandleAxisCommand(GamepadCommand command, float value)
    {
        switch (command)
        {
            case GamepadCommand.Move:
                if (!hasMoved)
                    controlledPlayer.MoveX(value);
                break;

            case GamepadCommand.Jump:
                controlledPlayer.Jump();
                break;

            case GamepadCommand.Transpose:
                break;

            case GamepadCommand.Interact:
                break;
        }
    }

    private void HandleTriggerCommand(GamepadCommand command, string trigger, float value)
    {
        if ((trigger.Contains("Right") && 0f < value) ||
            (trigger.Contains("Left") && value < 0f))
        {
            switch (command)
            {
                case GamepadCommand.Move:
                    if (!hasMoved)
                        controlledPlayer.MoveX(value);
                    break;

                case GamepadCommand.Jump:
                    controlledPlayer.Jump();
                    break;

                case GamepadCommand.Transpose:
                    if (!hasTransposed)
                    {
                        controlledPlayer.Transpose();
                        hasTransposed = true;
                    }
                    break;

                case GamepadCommand.Interact:
                    if (!hasInteracted)
                        controlledPlayer.Interact();
                    break;
            }
        }
    }
}

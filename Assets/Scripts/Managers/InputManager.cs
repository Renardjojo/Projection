using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum KeyboardCommand : byte
{
    MoveLeft = 0,
    MoveRight,
    Run,
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
    
    private Dictionary<KeyCode, KeyboardCommand>    keyboardInputs      = null;
    private Dictionary<string, GamepadCommand>      gamepadButtonInputs = null;
    private Dictionary<string, GamepadCommand>      gamepadAxisInputs   = null;

    // Prevent a single action from being executed twice (once wiht keyboard, once with gamepad)
    private bool hasMoved;
    private bool hasTransposed;
    private bool hasInteracted;

    private void Awake()
    {
        if (!controlledPlayer)
        {
            useKeyboard = useGamepad = false;
            Debug.Log("No player was passed");
        }

        if (useKeyboard)
        {
            // Initialize keyboard inputs
            keyboardInputs = new Dictionary<KeyCode, KeyboardCommand>();

            foreach (KeyValuePair<KeyboardCommand, KeyCode> kpv in keyboardControls)
                keyboardInputs.Add(kpv.Value, kpv.Key);
        }

        if (useGamepad)
        {
            // Initialize gamepad inputs
            gamepadButtonInputs = new Dictionary<string, GamepadCommand>();
            gamepadAxisInputs   = new Dictionary<string, GamepadCommand>();

            foreach (KeyValuePair<GamepadCommand, GamepadCode> kvp in gamepadControls)
            {
                // Button
                if (kvp.Value < GamepadCode.LeftHorizontal)
                    gamepadButtonInputs.Add(kvp.Value.ToString("g"), kvp.Key);

                // Axis
                else
                    gamepadAxisInputs.Add(kvp.Value.ToString("g"), kvp.Key);
            }
        }
    }


    private void Update()
    {
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
        {
            CMCam.Follow = controlledPlayer.controlledObject.transform;
        }

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

                if (useGamepad)
                    hasTransposed = true;
                break;

            case KeyboardCommand.Interact:
                controlledPlayer.Interact();
                if (useGamepad)
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
                if (keyboardControls.ContainsKey(KeyboardCommand.Run)
                    && Input.GetKey(keyboardControls[KeyboardCommand.Run]))
                {
                    // controlledPlayer.Sprint(-1f);
                    controlledPlayer.MoveX(-2f);
                }

                else
                    controlledPlayer.MoveX(-1f);

                if (useGamepad)
                    hasMoved = true;
                break;

            case KeyboardCommand.MoveRight:
                if (keyboardControls.ContainsKey(KeyboardCommand.Run)
                    && Input.GetKey(keyboardControls[KeyboardCommand.Run]))
                {
                    // controlledPlayer.Sprint(1f);
                    controlledPlayer.MoveX(2f);
                }

                else
                    controlledPlayer.MoveX(1f);

                if (useGamepad)
                    hasMoved = true;
                break;

            case KeyboardCommand.Jump:
                break;

            case KeyboardCommand.Transpose:
                break;

            case KeyboardCommand.Interact:
                break;

            case KeyboardCommand.ResetShadow:
                controlledPlayer.ResetShadow();
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
                if (useGamepad)
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

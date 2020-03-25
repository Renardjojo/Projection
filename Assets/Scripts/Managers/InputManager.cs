using System;
using System.Collections.Generic;
using UnityEngine;


public enum KeyboardCommand
{
    MoveLeft,
    MoveRight,
    Run,
    Jump,
    Dash,
    Switch,
    Interact
}

public enum GamepadCommand : byte
{
    Move = 0,
    Jump,
    Dash,
    Switch,
    Interact
}


public enum GamepadCode : byte
{
    A = 0,
    B,
    X,
    Y,
    Start,
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
    [SerializeField] private PlayerController       player              = null;
    [SerializeField] private bool                   useKeyboard         = true;
    [SerializeField] private bool                   useGamepad          = true;
    [SerializeField] private CommandKeyCodeDict     keyboardControls    = null;
    [SerializeField] private CommandGamepadCodeDict gamepadControls     = null;
    
    private Dictionary<KeyCode, KeyboardCommand>    keyboardInputs      = null;
    private Dictionary<string, GamepadCommand>      gamepadButtonInputs = null;
    private Dictionary<string, GamepadCommand>      gamepadAxisInputs   = null;

    private void Awake()
    {
        if (!player)
        {
            useKeyboard = useGamepad = false;
            Debug.Log("No player was passed");
        }


        if (useKeyboard)
        {
            keyboardInputs = new Dictionary<KeyCode, KeyboardCommand>();

            foreach (KeyValuePair<KeyboardCommand, KeyCode> kpv in keyboardControls)
                keyboardInputs.Add(kpv.Value, kpv.Key);
        }

        if (useGamepad)
        {
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

        Debug.Log(keyboardInputs);
    }


    private void Update()
    {
        if (useKeyboard)
        {
            // Listen to keyboard keys
            foreach (KeyValuePair<KeyCode, KeyboardCommand> kvp in keyboardInputs)
            {
                if      (Input.GetKey(kvp.Key))     HandlePressedKeyCommand(kvp.Value);
                else if (Input.GetKeyUp(kvp.Key))   HandleReleasedKeyCommand(kvp.Value);
            }
        }
        
        
        if (useGamepad)
        {
            // Listen to gamepad buttons
            foreach (KeyValuePair<string, GamepadCommand> kvp in gamepadButtonInputs)
            {
                if      (Input.GetButton(kvp.Key))      HandlePressedButtonCommand(kvp.Value);
                else if (Input.GetButtonUp(kvp.Key))    HandleReleasedButtonCommand(kvp.Value);
            }

            // Listen to gamepad axis
            foreach (KeyValuePair<string, GamepadCommand> kvp in gamepadAxisInputs)
                HandleAxisCommand(kvp.Value, Input.GetAxis(kvp.Key));
        }
        
    }


    // One of the keyboard input is pressed
    private void HandlePressedKeyCommand(KeyboardCommand command)
    {
        Debug.Log("Pressed : " + command.ToString("g"));

        switch (command)
        {
            case KeyboardCommand.MoveLeft:
                if (keyboardControls.ContainsKey(KeyboardCommand.Run)
                    && Input.GetKey(keyboardControls[KeyboardCommand.Run]))
                {
                    // pc.Sprint(-1f);
                }

                else
                    player.MoveX(-1f);
                break;

            case KeyboardCommand.MoveRight:
                if (keyboardControls.ContainsKey(KeyboardCommand.Run)
                    && Input.GetKey(keyboardControls[KeyboardCommand.Run]))
                {
                    // pc.Sprint(1f);
                }

                else
                    player.MoveX(1f);

                break;

            case KeyboardCommand.Jump:
                player.Jump(1f);
                break;

            case KeyboardCommand.Dash:
                // pc.Dash();
                break;

            case KeyboardCommand.Switch:
                player.Transpose();
                break;

            case KeyboardCommand.Interact:
                player.Interact();
                break;
        }
    }


    // One of the keyboard input is released
    private void HandleReleasedKeyCommand(KeyboardCommand command)
    {
        Debug.Log("Released : " + command.ToString("g"));

        switch (command)
        {
            case KeyboardCommand.MoveLeft:
                player.MoveX(0f);
                break;

            case KeyboardCommand.MoveRight:
                player.MoveX(0f);
                break;

            case KeyboardCommand.Jump:
                break;

            case KeyboardCommand.Dash:
                break;

            case KeyboardCommand.Switch:
                break;

            case KeyboardCommand.Interact:
                player.Interact();
                break;
        }
    }


    // One of the gamebad button input is pressed
    private void HandlePressedButtonCommand(GamepadCommand command)
    {
        switch (command)
        {
            case GamepadCommand.Move:
                player.MoveX(1f);
                break;

            case GamepadCommand.Jump:
                player.Jump(1f);
                break;

            case GamepadCommand.Dash:
                // pc.Dash();
                break;

            case GamepadCommand.Switch:
                player.Transpose();
                break;

            case GamepadCommand.Interact:
                break;
        }
    }


    // One of the gamebad button input is released
    private void HandleReleasedButtonCommand(GamepadCommand command)
    {
        switch (command)
        {
            case GamepadCommand.Move:
                break;

            case GamepadCommand.Jump:
                break;

            case GamepadCommand.Dash:
                break;

            case GamepadCommand.Switch:
                break;

            case GamepadCommand.Interact:
                player.Interact();
                break;
        }
    }


    // Check whether a command must be called
    private void HandleAxisCommand(GamepadCommand command, float value)
    {
        switch (command)
        {
            case GamepadCommand.Move:
                if (value != 0f) player.MoveX(value);
                break;

            case GamepadCommand.Jump:
                if (value != 0f) player.Jump(value);
                break;

            case GamepadCommand.Dash:
                // if (value != 0f) pc.Dash()
                break;

            case GamepadCommand.Switch:
                if (value != 0f) player.Transpose();
                break;

            case GamepadCommand.Interact:
                if (value != 0f) player.Interact();
                break;
        }
    }
}

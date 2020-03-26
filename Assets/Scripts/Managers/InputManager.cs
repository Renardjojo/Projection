using System;
using System.Collections.Generic;
using UnityEngine;


public enum KeyboardCommand : byte
{
    MoveLeft = 0,
    MoveRight,
    Run,
    Jump,
    Dash,
    Swap,
    Interact
}

public enum GamepadCommand : byte
{
    Move = 0,
    Jump,
    Dash,
    Swap,
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
    [SerializeField] private PlayerController       controlledPlayer    = null;
    [SerializeField] private bool                   useKeyboard         = true;
    [SerializeField] private bool                   useGamepad          = true;
    [SerializeField] private CommandKeyCodeDict     keyboardControls    = null;
    [SerializeField] private CommandGamepadCodeDict gamepadControls     = null;
    
    private Dictionary<KeyCode, KeyboardCommand>    keyboardInputs      = null;
    private Dictionary<string, GamepadCommand>      gamepadButtonInputs = null;
    private Dictionary<string, GamepadCommand>      gamepadAxisInputs   = null;

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
        if (useKeyboard)
        {
            // Listen to keyboard keys
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
                    HandleTriggerCommand(kvp.Key, Input.GetAxis(kvp.Key));
                else
                    HandleAxisCommand(kvp.Value, Input.GetAxis(kvp.Key));
            }
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
                break;

            case KeyboardCommand.Dash:
                break;

            case KeyboardCommand.Swap:
                controlledPlayer.Transpose();
                break;

            case KeyboardCommand.Interact:
                controlledPlayer.Interact();
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
                break;

            case KeyboardCommand.Jump:
                controlledPlayer.Jump(1f);
                break;

            case KeyboardCommand.Dash:
                /* controlledPlayer.Dash(); */
                break;

            case KeyboardCommand.Swap:
                break;

            case KeyboardCommand.Interact:
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
                break;

            case KeyboardCommand.Jump:
                break;

            case KeyboardCommand.Dash:
                break;

            case KeyboardCommand.Swap:
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
                controlledPlayer.MoveX(1f);
                break;

            case GamepadCommand.Jump:
                controlledPlayer.Jump(1f);
                break;

            case GamepadCommand.Dash:
                /* controlledPlayer.Dash(); */
                break;

            case GamepadCommand.Swap:
                controlledPlayer.Transpose();
                break;

            case GamepadCommand.Interact:
                controlledPlayer.Interact();
                break;
        }
    }


    // One of the gamepad button input is being pressed
    private void HandleButtonPressedCommand(GamepadCommand command)
    {
        switch (command)
        {
            case GamepadCommand.Move:
                break;

            case GamepadCommand.Jump:
                controlledPlayer.Jump(1f);
                break;

            case GamepadCommand.Dash:
                /* controlledPlayer.Dash(); */
                break;

            case GamepadCommand.Swap:
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
                controlledPlayer.MoveX(0f);
                break;

            case GamepadCommand.Jump:
                break;

            case GamepadCommand.Dash:
                break;

            case GamepadCommand.Swap:
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
                controlledPlayer.MoveX(value);
                break;

            case GamepadCommand.Jump:
                controlledPlayer.Jump(value);
                break;

            case GamepadCommand.Dash:
                // if (value != 0f) controlledPlayer.Dash()
                break;

            case GamepadCommand.Swap:
                break;

            case GamepadCommand.Interact:
                break;
        }
    }

    private void HandleTriggerCommand(string trigger, float value)
    {
        if (trigger.Contains("Left") && value < 0f)
        {
            // controlledPlayer.Dash()
        }

        else if (trigger.Contains("Left") && 0f < value)
        {
            controlledPlayer.Transpose();
        }
    }
}

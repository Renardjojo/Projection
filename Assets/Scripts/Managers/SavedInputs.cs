using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// If enabled, overrides the default InputManager settings depending on currentInputOptionsID.
public class SavedInputs : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager = null;

    [SerializeField]
    private int currentInputOptionsID = 0; // default

    [System.Serializable]
    class Preset
    {
        [SerializeField] public CommandKeyCodeDict keyboardControls = null;
        [SerializeField] public CommandGamepadCodeDict gamepadControls = null;
    }

    [SerializeField]
    List<Preset> presets = null;


    // Start is called before the first frame update
    void Start()
    {
        GameDebug.AssertInTransform(inputManager != null, transform, "input manager should not be null");

        currentInputOptionsID = PlayerPrefs.GetInt("inputs");

        LoadPreset();
    }

    private void LoadPreset()
    {
        if (inputManager != null)
        {
            if (currentInputOptionsID >= presets.Count)
                currentInputOptionsID = 0;

            inputManager.LoadGamepadControls(presets[currentInputOptionsID].gamepadControls);
            inputManager.LoadKeyboardControls(presets[currentInputOptionsID].keyboardControls);
        }
    }

    private void Update()
    {
        int presetID = PlayerPrefs.GetInt("inputs");
        if (presetID != currentInputOptionsID)
        {
            currentInputOptionsID = presetID;
            LoadPreset();
        }
    }
}

using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;

using UnityEngine.UI;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Button[] buttonList = null;

    private uint buttonIndex = 0; 


    // Start is called before the first frame update
    void Start()
    {
        
        //buttonList.OnTriggered += StartGame;
        //buttonList.OnTriggered += QuitGame;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeSelection(System.Math.Max(buttonIndex, 1) - 1); // to prevent doing 0 - 1
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeSelection(System.Math.Min(buttonIndex + 1, uint.MaxValue));
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            RunButton();
        }

        //GameObject myEventSystem = GameObject.Find("EventSystem");
        //myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(buttonList[buttonIndex].gameObject);
    }

    void ChangeSelection(uint newIndex)  
    {
        if (newIndex >= buttonList.Length)
            newIndex = (uint)buttonList.Length - 1u;
        //Mathf.Clamp(newIndex, 0, buttonList.Length - 1);

        // Checking bounds
        if (newIndex == buttonIndex)
            return;

        Debug.Log(buttonIndex + " / " + newIndex + " / " + buttonList.Length);
        Unhowever(buttonList[buttonIndex]);
        buttonIndex = newIndex;
        However(buttonList[buttonIndex]);
    }

    void However(UnityEngine.UI.Button button)
    {
        FadeButtonColor(button, button.colors.normalColor);
    }

    void Unhowever(UnityEngine.UI.Button button)
    {
        FadeButtonColor(button, button.colors.pressedColor);
    }

    static private void FadeButtonColor(UnityEngine.UI.Button button, Color finalColor)
    {
        button.targetGraphic.CrossFadeColor(finalColor, button.colors.fadeDuration, true, true);
    }

    private void RunButton()
    {
        buttonList[buttonIndex].onClick?.Invoke();
    }
}

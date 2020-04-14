using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMenu : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Button button = null;

    public void EnableMenu()
    {
        // Reset selection to be sure we select the button and set its color
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null, null);
        gameObject.SetActive(true);

        if (button != null)
        {
            button.Select();
            //UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(button.gameObject, null);
        }
    }

    public void DisableMenu()
    {
        gameObject.SetActive(false);
    }

    public void EnableMenu(bool shouldEnable)
    {
        if (shouldEnable)
            EnableMenu();
        else
            DisableMenu();
    }

    public void Switch()
    {
        EnableMenu(!gameObject.activeSelf);
    }
}

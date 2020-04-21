using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class EventSystemExtra : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem = null;
    [SerializeField] private Color unselectedColor = new Color(0.44f, 0.43f, 0.43f);
    [SerializeField] private Color selectedColor = Color.white;

    private GameObject lastSelection = null;

    private UnityEngine.UI.Text GetTextFromButton()
    {
        if (lastSelection == null)
            return null;

        UnityEngine.UI.Button button = lastSelection.GetComponent<UnityEngine.UI.Button>();

        if (button != null)
        {
            return lastSelection.GetComponentInChildren<UnityEngine.UI.Text>();
        }
        else
            return null;
    }

    private void RemoveSelectionStyle()
    {
        if (lastSelection == null)
            return;

        UnityEngine.UI.Text text = GetTextFromButton();
        if (text != null)
        {
            text.color = unselectedColor;
        }
    }

    private void AddSelectionStyle()
    {
        if (lastSelection == null)
            return;

        UnityEngine.UI.Text text = GetTextFromButton();
        if (text != null)
        {
            text.color = selectedColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lastSelection != EventSystem.current)
        {
            RemoveSelectionStyle();
            lastSelection = EventSystem.current.currentSelectedGameObject;
            AddSelectionStyle();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Similar to the default EventSystem of Unity, except we can also change text's style.
public class CustomEventSystem : MonoBehaviour
{
    [SerializeField] private Color unselectedColor = Color.white;
    [SerializeField] private Color selectedColor   = Color.green;

    [SerializeField] private bool   isVertical = true;
    [SerializeField] private string vertical   = "Debug Vertical";
    [SerializeField] private string horizontal = "Debug Horizontal";
    [SerializeField] private string submit     = "Debug Submit";
    [SerializeField] private float  delay      = 0.5f;
    [SerializeField] private float  dead       = 0.1f;
    private float  timeTillMoveTick;

    [SerializeField]
    private UnityEngine.UI.Selectable[] selectables = null;

    private UnityEngine.UI.Text[] texts = null;

    uint currentSelection = 0;

    private void Awake()
    {
        texts = new UnityEngine.UI.Text[selectables.Length];
        for (int i = 0; i < selectables.Length; i++)
        {
            if (selectables[i] != null)
                texts[i] = selectables[i].GetComponentInChildren<UnityEngine.UI.Text>();
        }

        timeTillMoveTick = delay;

        if (!IsValidButton(currentSelection))
            TakeNextSelectionTillValid();

        AddStyle(currentSelection);
    }

    private bool IsValidButton(uint i)
    {
        return (selectables[i] != null && selectables[i].gameObject.activeSelf);
    }

    private void TakeNextSelectionTillValid()
    {
        uint i = Clamp(currentSelection, 0u, ((uint)selectables.Length) - 2u);

        do
        {
            i++;
        } while (i < selectables.Length && !IsValidButton(i));

        if (i < selectables.Length && IsValidButton(i))
        {
            currentSelection = i;   
        }
    }

    uint Clamp(uint value, uint min, uint max)
    {
        return (value < min) ? min : (value > max ? max : value);
    }

    private void TakePreviousSelectionTillValid()
    {
        uint i = Clamp(currentSelection, 1u, ((uint)selectables.Length) - 1u);

        do
        {
            i--;
        } while ((i > 0 && !IsValidButton(i)));

        if (i >= 0 && IsValidButton(i))
        {
            currentSelection = i;
        }
    }

    private void RemoveStyle(uint i)
    {
        if (i >= 0 && i < texts.Length && IsValidButton(i))
        {
            if (texts[i] != null && texts[i].color != null)
            {
                texts[i].color = unselectedColor;
            }
        }
    }

    private void AddStyle(uint i)
    {
        if (IsValidButton(i))
        {
            if (texts[i] != null && texts[i].color != null)
            {
                texts[i].color = selectedColor;
            }
        }
    }

    public void SelectNext()
    {
        RemoveStyle(currentSelection);
        TakeNextSelectionTillValid();
        AddStyle(currentSelection);
    }

    public void SelectPrevious()
    {
        RemoveStyle(currentSelection);
        TakePreviousSelectionTillValid();
        AddStyle(currentSelection);
    }

    private void ChangeSelection(int newSelection)
    {
        while (currentSelection < selectables.Length && (selectables[currentSelection] == null || !selectables[currentSelection].enabled))
        {
            currentSelection++;
        }
    }

    private void Update()
    {
        if (Input.GetAxis(vertical) > dead)
        {
            if (delay <= timeTillMoveTick || timeTillMoveTick <= 0f)
                SelectPrevious();
            
            timeTillMoveTick -= Time.unscaledDeltaTime;
        }
        else if (Input.GetAxis(vertical) < -dead)
        {
            if (delay <= timeTillMoveTick || timeTillMoveTick <= 0f)
                SelectNext();
            
            timeTillMoveTick -= Time.unscaledDeltaTime;
        }
        else
        {
            timeTillMoveTick = delay;
        }

        if (Input.GetButtonDown(submit) && currentSelection >= 0 && currentSelection < selectables.Length)
        {
            //selectables[currentSelection].onClick.Invoke();
            UnityEngine.UI.Button button = selectables[currentSelection] as UnityEngine.UI.Button;
            if (button != null)
            {
                button.onClick.Invoke();
                return;
            }

            UnityEngine.UI.Slider slider = selectables[currentSelection] as UnityEngine.UI.Slider;
            if (slider != null)
            {
                //slider.value += 
                return;
            }

            UnityEngine.UI.Dropdown dropDown = selectables[currentSelection] as UnityEngine.UI.Dropdown;
            if (dropDown != null)
            {
                dropDown.Show();
                return;
            }
        }
    }
}

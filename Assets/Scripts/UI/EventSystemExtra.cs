using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class EventSystemExtra : MonoBehaviour
{
    [SerializeField] private Color unselectedColor = new Color(0.44f, 0.43f, 0.43f);
    [SerializeField] private Color selectedColor   = Color.white;
    [SerializeField] private float menuVolume = 0.5f;

    [SerializeField] private AudioClip changeSelectionClip   = null;
    private AudioSource                changeSelectionSource = null;

    [SerializeField] 
    private AudioClip   submitClip   = null;
    private AudioSource submitSource = null;

    private GameObject lastSelection = null;

    private void Awake()
    {
        changeSelectionSource = gameObject.AddComponent<AudioSource>();
        changeSelectionSource.clip = changeSelectionClip;
        changeSelectionSource.volume = menuVolume;

        submitSource = gameObject.AddComponent<AudioSource>();
        submitSource.clip = submitClip;
        submitSource.volume = menuVolume;
    }

    public void PlaySubmitSound()
    {
        submitSource.Play();
    }

    public void PlaySelectionSound()
    {
        changeSelectionSource.Play();
    }

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

        changeSelectionSource.Play();
    }
                                                                  
    // Update is called once per frame
    void Update()
    {
        if (lastSelection != EventSystem.current.currentSelectedGameObject)
        {
            RemoveSelectionStyle();
            lastSelection = EventSystem.current.currentSelectedGameObject;
            AddSelectionStyle();

            if (lastSelection != null)
            {
                Selectable selectable = lastSelection.GetComponent<Selectable>();
                Navigation nav = selectable.navigation;
                {
                    Selectable currentNext = selectable.navigation.selectOnDown;
                    while (currentNext != null && !currentNext.gameObject.activeSelf)
                    {
                        currentNext = currentNext.navigation.selectOnDown;
                    }
                    nav.selectOnDown = currentNext;
                }
                {
                    Selectable currentNext = selectable.navigation.selectOnUp;
                    while (currentNext != null && !currentNext.gameObject.activeSelf)
                    {
                        currentNext = currentNext.navigation.selectOnUp;
                    }
                    nav.selectOnUp = currentNext;
                }
                selectable.navigation = nav;
            }
        }
    }
}

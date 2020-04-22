using System.Collections;
using UnityEngine;

public class SubMenu : MonoBehaviour
{
    [SerializeField, Tooltip("First Selected Button")]
    private UnityEngine.UI.Button button = null;
    [SerializeField, Range(0.1f, 50f)] float fadeOutSpeed = 0.8f;
    Coroutine fadeCoroutine = null;
    internal bool isActivate = false;

    private void Awake()
    {
        isActivate = gameObject.activeSelf;
    }

    public void EnableMenu()
    {
        gameObject.SetActive(true);
        isActivate = true;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeCoroutine(true));
    }

    public void DisableMenu()
    {
        if (!gameObject.activeSelf)
            return;

        isActivate = false;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeCoroutine(false));
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

    IEnumerator FadeCoroutine(bool InOrOut) // true for in
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        if (InOrOut)
        {
            while ((canvasGroup.alpha += fadeOutSpeed * Time.unscaledDeltaTime) < 1f)
            {
                yield return null;
            }

            // Reset selection to be sure we select the button and set its color
            if (UnityEngine.EventSystems.EventSystem.current != null)
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null, null);

            if (button != null)
            {
                button.Select();
                //UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(button.gameObject, null);
            }
        }
        else
        {
            while ((canvasGroup.alpha -= fadeOutSpeed * Time.unscaledDeltaTime) > 0f)
            {
                yield return null;
            }

            gameObject.SetActive(false);
        }

    }
}

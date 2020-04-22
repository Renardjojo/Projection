using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    bool isOnHub = false;
    [SerializeField, Range (0.1f, 1f)] float fadeOutSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        string newScene;

        // Tutorial already completed
        if (PlayerPrefs.GetInt("Tutorial") == 1)
        {
            newScene = "HUB";
            isOnHub = true;
        }

        // Tutorial not yet completed
        else
        {
            newScene = "Tutorial";
            isOnHub = false;
        }

        if (Application.CanStreamedLevelBeLoaded(newScene))
        {
            SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
        }
        else
        {
            Debug.LogError("The Main Scene could not be loaded." +
                "Make sure " + newScene + " exists."
                + "also, check file -> build settings to make sure it is valid.");
        }

         StartCoroutine(WaitForSceneLoad(SceneManager.GetSceneByName(newScene)));
    }   

    IEnumerator WaitForSceneLoad(Scene scene)
    {
        while (!scene.isLoaded)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(scene);
        GameObject.Find("GameManager/Manager/InputManager").SetActive(false);
    }

    IEnumerator FadeOutCoroutine()
    {
        CanvasGroup canvasGroup = GameObject.Find("TitleScreen").GetComponent<CanvasGroup>();

        while ((canvasGroup.alpha -= fadeOutSpeed * Time.deltaTime) > 0f)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync("MainMenu");
        GameObject.Find("GameManager/Manager/InputManager").SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey )
        {
            StartCoroutine(FadeOutCoroutine());
        }
    }
}

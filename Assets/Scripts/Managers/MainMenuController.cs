using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    bool isOnHub = false;

    // Start is called before the first frame update
    void Start()
    {
        string newScene;

        if (PlayerPrefs.GetInt("PlayerHaveLoadTutoScene") == 1)
        {
            newScene = "HUB";
            isOnHub = true;
        }
        else
        {
            newScene = "Tutorial";
            PlayerPrefs.SetInt("PlayerHaveLoadTutoScene", 1);
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

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey )
        {
            SceneManager.UnloadSceneAsync("MainMenu");

            GameObject.Find("GameManager/Manager/InputManager").SetActive(true);
        }
    }
}

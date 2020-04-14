using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string newScene;

        if (PlayerPrefs.GetInt("PlayerHaveLoadTutoScene") == 1)
        {
            newScene = "HUB";
        }
        else
        {
            newScene = "Tutorial";
            PlayerPrefs.SetInt("PlayerHaveLoadTutoScene", 1);
        }

        if (Application.CanStreamedLevelBeLoaded(newScene))
        {
            SceneManager.LoadScene(newScene, LoadSceneMode.Single);
        }
        else
        {
            Debug.LogError("The Main Scene could not be loaded." +
                "Make sure " + newScene + " exists."
                + "also, check file -> build settings to make sure it is valid.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

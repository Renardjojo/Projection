using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    private string startGameScene = "Prototype5";

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        if (Application.CanStreamedLevelBeLoaded(startGameScene))
        {
            LoadNewScene(startGameScene);
        }
        else
        {
            Debug.LogError("The Main Scene could not be loaded." +
                "Make sure " + startGameScene + " exists."
                + "also, check file -> build settings to make sure it is valid.");
        }
    }

    internal void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}

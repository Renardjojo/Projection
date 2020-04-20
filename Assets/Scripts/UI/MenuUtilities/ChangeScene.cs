using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void TryToLoadScene(string newScene)
    {
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
}

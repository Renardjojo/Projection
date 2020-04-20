using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField]
    private string newScene = "Prototype4";

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.UI.Button btn = GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (Application.CanStreamedLevelBeLoaded(newScene))
        {
            Time.timeScale = 1f;
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

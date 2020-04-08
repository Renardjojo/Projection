using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddSceneButton : MonoBehaviour
{
    [SerializeField]
    private string addedSceneName = "InGameMenu";

    private Scene scene;

    private void Awake()
    {
        UnityEngine.UI.Button btn = GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(Load);
    }

    void Load()
    {
        if (!scene.isLoaded) // to prevent multiple scenes from being spawned
        {
            LoadSceneParameters param = new LoadSceneParameters(LoadSceneMode.Additive);
            scene = SceneManager.LoadScene(addedSceneName, param);
        }
    }
}

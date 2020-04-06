using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddScene : MonoBehaviour
{
    [SerializeField]
    private string addedSceneName = "InGameMenu";

    private Scene scene;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Load();
        }
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

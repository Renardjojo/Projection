using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class QuitButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Quit);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Quit()
    {
        Debug.Log("Game is leaving (note : it only occurs in release build)");
        Application.Quit();
    }
}

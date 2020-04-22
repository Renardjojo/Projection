using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    internal Coroutine cameraZoomCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopCameraZoomCoroutine()
    {
        if (cameraZoomCoroutine != null)
        {
            StopCoroutine(cameraZoomCoroutine);
        }
    }

    public void StartCameraZoomCoroutine(IEnumerator corroutine)
    {
        cameraZoomCoroutine = StartCoroutine(corroutine);
    }
}

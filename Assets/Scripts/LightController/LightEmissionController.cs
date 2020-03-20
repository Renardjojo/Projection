using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class LightEmissionController : MonoBehaviour
{
    [SerializeField] private Color      onColor = Color.red;
    [SerializeField] private Color      offColor = Color.green;
                     private Material   material;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        material.SetColor("_EmissionColor", onColor);
        material.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOn()
    {
        material.SetColor("_EmissionColor", onColor);
    }

    public void SetOff()
    {
        material.SetColor("_EmissionColor", offColor);
    }
}

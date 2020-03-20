using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class LightEmissionController : MonoBehaviour
{
    [SerializeField] private Color      onColor     = Color.red;
    [SerializeField] private Color      offColor    = Color.green;
    [SerializeField]
    [Range(0.001f, 1f)]
    [Tooltip("Define lerp speed by second. This value move from 0 to 1. Lerp at 0 is not possible. Lerp at 1 chang immediatly the color")]
                    private float      lerpSpeed   = 1f;
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

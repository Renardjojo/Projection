using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class LightEmissionController : MonoBehaviour
{
    [SerializeField] private Material onColor;
    [SerializeField] private Material offColor;
                     private Renderer renderer;

    // Start is called before the first frame update
    void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        renderer.sharedMaterial = offColor;
    }

    public void SetOn()
    {
        renderer.sharedMaterial = onColor;
    }

    public void SetOff()
    {
        renderer.sharedMaterial = offColor;
    }
}

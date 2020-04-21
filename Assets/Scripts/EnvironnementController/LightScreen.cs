using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScreen : MonoBehaviour
{
    private PlayerController pc;

    [SerializeField] private GameObject plan1;
    [SerializeField] private GameObject planStencil;

    [SerializeField] private Material onMaterial  = null; 
    [SerializeField] private Material offMaterial = null;

    private Renderer screenRenderer = null;
    private BoxCollider screenCollider = null;

    private void Awake()
    {
        pc = GameObject.FindObjectOfType<PlayerController>();

        screenRenderer = plan1.GetComponent<Renderer>();
        screenCollider = plan1.GetComponent<BoxCollider>();
    }

    void OnActivation()
    {
        if (pc.IsShadowCollidingWithLightScreen())
        {
            pc.ResetShadow();
        }
    }

    public void SetActivation(bool flag)
    {
        //plan1.SetActive(flag);
        planStencil.SetActive(flag);
        screenCollider.enabled = flag;

        if (flag)
        {
            OnActivation();
            screenRenderer.material = onMaterial;
        }
        else
        {
            screenRenderer.material = offMaterial;
        }
    }
}

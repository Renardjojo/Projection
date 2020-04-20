using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScreen : MonoBehaviour
{
    private PlayerController pc;

    [SerializeField] private GameObject plan1;
    [SerializeField] private GameObject plan2;

    private void Awake()
    {
        pc = GameObject.FindObjectOfType<PlayerController>();
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
        plan1.SetActive(flag);
        plan2.SetActive(flag);

        if (flag)
        {
            OnActivation();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class LightEmissionController : MonoBehaviour
{
    [SerializeField] private Color      onColor     = Color.green;
    [SerializeField] private Color      offColor    = Color.red;
    //[SerializeField]
    //[Range(0.001f, 1f)]
    //[Tooltip("Define lerp speed by second. This value move from 0 to 1. Lerp at 0 is not possible. Lerp at 1 chang immediatly the color")]
    //                private float      lerpSpeed   = 1f;
                    private Material   material;

    // Start is called before the first frame update
    void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        material.SetColor("_EmissionColor", offColor);
        material.EnableKeyword("_EMISSION");
        material.color = Color.black;
    }

    public void SetOn()
    {
        //if (lerpSpeed == 1f)
        //{
            material.SetColor("_EmissionColor", onColor);
        //}
        //else
        //{
        //    StopAllCoroutines();
        //    StartCoroutine(EmissionLerpCorroutine(true));
        //}
    }

    public void SetOff()
    {
        //if (lerpSpeed == 1f)
        //{
            material.SetColor("_EmissionColor", offColor);
        //}
        //else
        //{
        //    StopAllCoroutines();
        //    StartCoroutine(EmissionLerpCorroutine(true));
        //}
    }

    //private IEnumerator EmissionLerpCorroutine(bool OnTowardOff)
    //{
    //    float lerpStep = 0f;
    //    yield return new WaitWhile(EmissionLerp(lerpStep, OnTowardOff));
    //}

    //bool EmissionLerp(float step, bool OnTowardOff)
    //{
    //    if (OnTowardOff)
    //    material.SetColor("_EmissionColor", onColor);
    //    return step < 1f;
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Collider))]
public class CameraGeneralPlan : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;
    private Collider collider;

    [SerializeField, Range(1f, 15f)] private float generalPlanScale = 7f;
    [SerializeField, Range(0f, 1f)] private float lerpSpeed = 0.8f;
    float exPlanScale = 5f; 

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (camera.Follow.gameObject == other.gameObject)
        {
            exPlanScale = camera.m_Lens.OrthographicSize;
            StopAllCoroutines();
            StartCoroutine(LerpScale(generalPlanScale));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (camera.Follow.gameObject == other.gameObject)
        {
            StopAllCoroutines();
            StartCoroutine(LerpScale(exPlanScale));
        }
    }

    IEnumerator LerpScale(float lerpGaol)
    {
        while (Mathf.Abs(camera.m_Lens.OrthographicSize - lerpGaol) > 0.01f) //Espilone
        {
            camera.m_Lens.OrthographicSize = Mathf.Lerp(camera.m_Lens.OrthographicSize, lerpGaol, lerpSpeed * Time.deltaTime);
            yield return null;
        }

        camera.m_Lens.OrthographicSize = lerpGaol;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Collider))]
public class CameraGeneralPlan : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private ZoomCameraBetweenEntities zoomCameraPlayerScript;
    private Collider collider;

    [SerializeField, Range(1f, 15f)] private float generalPlanScale = 7f;
    [SerializeField, Range(0f, 1f)] private float lerpSpeed = 0.8f;
    float exPlanScale;


    private void Awake()
    {
        collider = GetComponent<Collider>();
        gameObject.layer = LayerMask.NameToLayer("TransparentFX");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (camera.Follow.gameObject == other.gameObject)
        {
            exPlanScale = zoomCameraPlayerScript.minOrthoSize;
            zoomCameraPlayerScript.minOrthoSize = generalPlanScale;
            StopAllCoroutines();
            StartCoroutine(LerpScale(generalPlanScale));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (camera.Follow.gameObject == other.gameObject)
        {
            zoomCameraPlayerScript.minOrthoSize = exPlanScale;
            StopAllCoroutines();
            StartCoroutine(LerpScale(exPlanScale));
        }
    }

    public IEnumerator LerpScale(float lerpGaol)
    {
        float lerpStep = 0f;

        zoomCameraPlayerScript.SetActivate(false);
        while (lerpStep < 1f) //Espilone
        {
            lerpStep += lerpSpeed * Time.unscaledDeltaTime;
            camera.m_Lens.OrthographicSize = Mathf.Lerp(camera.m_Lens.OrthographicSize, lerpGaol, lerpStep);
            yield return null;
        }

        zoomCameraPlayerScript.SetActivate(true);
        camera.m_Lens.OrthographicSize = lerpGaol;
    }
}

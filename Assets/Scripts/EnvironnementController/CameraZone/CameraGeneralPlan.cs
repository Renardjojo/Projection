using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Collider))]
public class CameraGeneralPlan : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField, Range(1f, 25f)] private float generalPlanScale = 7f;

    private CameraManager cameraManager;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("TransparentFX");
        cameraManager = GameObject.Find("GameManager/Manager/CinemachineManager").GetComponent<CameraManager>();
        if (cameraManager == null)
            Debug.LogAssertion("CinemachineManager not found");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (camera.Follow.gameObject == other.gameObject)
        {
            cameraManager.PlayerEnterInCameraZone(generalPlanScale);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (camera.Follow.gameObject == other.gameObject)
        {
            cameraManager.PlayerExitCameraZone();
        }
    }

}

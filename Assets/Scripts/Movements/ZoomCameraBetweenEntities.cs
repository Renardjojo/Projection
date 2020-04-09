using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZoomCameraBetweenEntities : MonoBehaviour
{
    [SerializeField] GameObject                 mainEntity;
    [SerializeField] GameObject                 secondEntity;
    [SerializeField] CinemachineVirtualCamera   cinemachineVirtualCamera;
    [SerializeField] float distanceOffSet = 5f;
    float normalSize;

    bool isActivate = true;


    // Start is called before the first frame update
    void Start()
    {
        normalSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivate)
            return;

        float mainGOToSecondaryGO = (secondEntity.transform.position - mainEntity.transform.position).magnitude;

        if (mainGOToSecondaryGO - distanceOffSet > cinemachineVirtualCamera.m_Lens.OrthographicSize)
        {
            cinemachineVirtualCamera.m_Lens.OrthographicSize = mainGOToSecondaryGO + distanceOffSet;
        }
        else if (mainGOToSecondaryGO > normalSize)
        {
            cinemachineVirtualCamera.m_Lens.OrthographicSize = mainGOToSecondaryGO;
        }
    }

    public void EnableCameraZoom ()
    {
        isActivate = true;
    }

    public void DisableCameraZoom ()
    {
        isActivate = false;
        cinemachineVirtualCamera.m_Lens.OrthographicSize = normalSize;
    }

    public void SwitchCameraZoomState ()
    {
        if (isActivate)
        {
            DisableCameraZoom();
        }
        else
        {
            EnableCameraZoom();
        }
    }
}

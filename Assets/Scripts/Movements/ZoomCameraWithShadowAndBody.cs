using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZoomCameraWithShadowAndBody : MonoBehaviour
{
    [SerializeField] GameObject                 bodyPlayer;
    [SerializeField] GameObject                 shadowPlayer;
    [SerializeField] CinemachineVirtualCamera   cinemachineVirtualCamera;
    [SerializeField] float distanceOffSet = 5f;
    float normalSize;

    PlayerController playerControllerScript;


    // Start is called before the first frame update
    void Start()
    {
        normalSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;

        playerControllerScript = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.shadowProperties.activateShadow)
        {
            float bodyToShadow = (shadowPlayer.transform.position - bodyPlayer.transform.position).magnitude;

            if (bodyToShadow - distanceOffSet > cinemachineVirtualCamera.m_Lens.OrthographicSize)
            {
                cinemachineVirtualCamera.m_Lens.OrthographicSize = bodyToShadow + distanceOffSet;
            }
            else if (bodyToShadow > normalSize)
            {
                cinemachineVirtualCamera.m_Lens.OrthographicSize = bodyToShadow;
            }
        }
        else
        {
            cinemachineVirtualCamera.m_Lens.OrthographicSize = normalSize;
        }
    }
}

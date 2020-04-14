using UnityEngine;
using Cinemachine;

public class ZoomCameraBetweenEntities : MonoBehaviour
{
    [SerializeField] GameObject                 mainEntity      = null;
    [SerializeField] GameObject                 secondEntity    = null;
    [SerializeField] CinemachineVirtualCamera   cam             = null;
    [SerializeField] float                      distanceOffset  = 5f;

    float minOrthoSize;

    void Awake()
    {
        Debug.Assert(cam, "Camera has not been set in component \"Zoom Camera Between Entities\"");
        minOrthoSize = cam.m_Lens.OrthographicSize;
    }


    void Update()
    {
        float mainToSecondary = (secondEntity.transform.position - mainEntity.transform.position).magnitude;

        cam.m_Lens.OrthographicSize = (distanceOffset < mainToSecondary) ? mainToSecondary : minOrthoSize;
    }
}

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
        if (!cam)
            cam = FindObjectOfType<CinemachineVirtualCamera>();

        Debug.Assert(cam, "Camera has not been set in component \"Zoom Camera Between Entities\"");
        minOrthoSize = cam.m_Lens.OrthographicSize;
    }


    void Update()
    {
        float mainToSecondary = (secondEntity.transform.position - mainEntity.transform.position).magnitude;

        if (mainToSecondary <= distanceOffset)
            cam.m_Lens.OrthographicSize = minOrthoSize;

        // The orthographic size is the half height of the camera view.
        // It is multiplied by .5f * 1f / cam.m_Lens.Aspect to keep the player and
        // the shadow on screen, while not diszooming too much
        else
            cam.m_Lens.OrthographicSize = minOrthoSize + (mainToSecondary - distanceOffset) * 1f / cam.m_Lens.Aspect;

        //Debug.Log("cam.m_Lens.OrthographicSize = " + cam.m_Lens.OrthographicSize);
    }
}

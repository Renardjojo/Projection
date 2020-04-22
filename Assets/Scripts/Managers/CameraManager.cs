using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraManager : MonoBehaviour
{
    private CinemachineVirtualCamera camera;
    [SerializeField] GameObject mainEntity      = null;
    [SerializeField] GameObject secondEntity    = null;
    [SerializeField] float      distanceOffsetZoomBetweenEntity = 5f;

    private int         playerOnCameraZoneEnterCount = 0;
    internal float      cameraZoneGoal = 0f;
    
    float neutralCameraSize;

    /*
    [SerializeField, Range(0f, 100f)] float lightOffIntensity;
    [SerializeField, Range(0f, 100f)] float lightOnIntensity;
    [SerializeField] AnimationCurve lightCurveFallOut;
    [SerializeField, Range(1f, 10f)] float fallOutDelay = 1f;
    */

    float cameraZoomBetweenEntityGoal;

    private void Awake()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        neutralCameraSize = cameraZoomBetweenEntityGoal = camera.m_Lens.OrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraZoomBetweenEntity();

        if (playerOnCameraZoneEnterCount != 0)
        {
            if (cameraZoomBetweenEntityGoal > cameraZoneGoal)
            {
                ScaleOrUnscaleCamera(cameraZoomBetweenEntityGoal);
            }
            else
            {
                ScaleOrUnscaleCamera(cameraZoneGoal);
            }
        }
        else
        {
            if (cameraZoomBetweenEntityGoal > neutralCameraSize)
            {
                ScaleOrUnscaleCamera(cameraZoomBetweenEntityGoal);
            }
            else
            {
                ScaleOrUnscaleCamera(neutralCameraSize);
            }
        }
    }

    public void ScaleOrUnscaleCamera(float cameraScale)
    {
        camera.m_Lens.OrthographicSize = Mathf.Lerp(camera.m_Lens.OrthographicSize, cameraScale, Time.unscaledDeltaTime);
    }

    public void UpdateCameraZoomBetweenEntity()
    {
        float mainToSecondary = (secondEntity.transform.position - mainEntity.transform.position).magnitude;

        // The orthographic size is the half height of the camera view.
        // It is multiplied by 1f / cam.m_Lens.Aspect to keep the player and
        // the shadow on screen horizontally, while not unzooming too much
        cameraZoomBetweenEntityGoal = neutralCameraSize + (mainToSecondary - distanceOffsetZoomBetweenEntity) * 1f / camera.m_Lens.Aspect;
    }

    public void PlayerEnterInCameraZone (float cameraScaleGoal)
    {
        playerOnCameraZoneEnterCount++;
        cameraZoneGoal = cameraScaleGoal;
    }

    public void PlayerExitCameraZone()
    {
        playerOnCameraZoneEnterCount--;

        if(playerOnCameraZoneEnterCount < 0)
        {
            playerOnCameraZoneEnterCount = 0;
        }
    }
}
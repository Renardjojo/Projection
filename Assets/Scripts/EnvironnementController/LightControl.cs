using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum LightControlState
{
    On, 
    Off, 
    FallOutOnToOff, 
    FallOutOffToOn
}

public class LightControl : MonoBehaviour
{
    [SerializeField] Light lightToControl;

    [SerializeField] LightControlState lightControlState = LightControlState.On;

    [SerializeField, Range(0f, 100f)] float lightOffIntensity;
    [SerializeField, Range(0f, 100f)] float lightOnIntensity;
    [SerializeField]                  AnimationCurve lightCurveFallOut;
    [SerializeField, Range(1f, 10f)]  float fallOutDelay = 1f;

    float step = 0f;

    // Start is called before the first frame update
    void Start()
    {
        switch (lightControlState)
        {
            case LightControlState.On:
                lightToControl.intensity = lightOnIntensity;
                step = 1f;
                break;
            case LightControlState.FallOutOnToOff:
                step = 1f;
                break;

            case LightControlState.Off:
                lightToControl.intensity = lightOffIntensity;
                step = 0f;
                break;
            case LightControlState.FallOutOffToOn:
                step = 0f;
                break;

            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (lightControlState)
        {
            case LightControlState.FallOutOnToOff:
                step -= (Time.deltaTime / fallOutDelay) * Time.timeScale;

                if (step <= 0f)
                {
                    step = 0f;
                    lightControlState = LightControlState.Off;
                }

                lightToControl.intensity = lightCurveFallOut.Evaluate(step) * (lightOnIntensity - lightOffIntensity) + lightOffIntensity;
                break;

            case LightControlState.FallOutOffToOn:
                step += (Time.deltaTime / fallOutDelay) * Time.timeScale;

                if (step >= 1f)
                {
                    step = 1f;
                    lightControlState = LightControlState.On;
                }

                lightToControl.intensity = lightCurveFallOut.Evaluate(step) * (lightOnIntensity - lightOffIntensity) + lightOffIntensity;
                break;

            default:
                break;
        }
    }

    public void SetLightActivate(bool flag)
    {
        if (flag)
        {
            lightControlState = LightControlState.FallOutOffToOn;
        }
        else
        {
            lightControlState = LightControlState.FallOutOnToOff;
        }
    }
}

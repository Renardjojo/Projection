using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum TextControlState
{
    FallOutOnToOff,
    FallOutOffToOn
}

[RequireComponent(typeof(Text))]
public class TextColorControl : MonoBehaviour
{
    Text text;

    [SerializeField] Color  FinalColor;
    Color                   StartColor;
    [SerializeField] AnimationCurve lightCurveFallOut;
    [SerializeField, Range(1f, 10f)]  float fallOutDelay = 1f;

    TextControlState controlState = TextControlState.FallOutOnToOff;

        float step = 0f;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        StartColor = text.color;

        switch (controlState)
        {
            case TextControlState.FallOutOnToOff:
                step = 1f;
                break;

            case TextControlState.FallOutOffToOn:
                step = 0f;
                break;

            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (controlState)
        {
            case TextControlState.FallOutOnToOff:
                step -= (Time.deltaTime / fallOutDelay) * Time.timeScale;

                if (step <= 0f)
                {
                    step = 0f;
                    controlState = TextControlState.FallOutOffToOn;
                }

                text.color = new Color( lightCurveFallOut.Evaluate(step) * (StartColor.r - FinalColor.r) + FinalColor.r,
                                        lightCurveFallOut.Evaluate(step) * (StartColor.g - FinalColor.g) + FinalColor.g,
                                        lightCurveFallOut.Evaluate(step) * (StartColor.b - FinalColor.b) + FinalColor.b,
                                        lightCurveFallOut.Evaluate(step) * (StartColor.a - FinalColor.a) + FinalColor.a);
                break;

            case TextControlState.FallOutOffToOn:
                step += (Time.deltaTime / fallOutDelay) * Time.timeScale;

                if (step >= 1f)
                {
                    step = 1f;
                    controlState = TextControlState.FallOutOnToOff;
                }

                text.color = new Color(lightCurveFallOut.Evaluate(step) * (StartColor.r - FinalColor.r) + FinalColor.r,
                                        lightCurveFallOut.Evaluate(step) * (StartColor.g - FinalColor.g) + FinalColor.g,
                                        lightCurveFallOut.Evaluate(step) * (StartColor.b - FinalColor.b) + FinalColor.b,
                                        lightCurveFallOut.Evaluate(step) * (StartColor.a - FinalColor.a) + FinalColor.a);
                break;

            default:
                break;
        }
    }

    public void SetActivate(bool flag)
    {
        if (flag)
        {
            controlState = TextControlState.FallOutOffToOn;
        }
        else
        {
            controlState = TextControlState.FallOutOnToOff;
        }
    }
}

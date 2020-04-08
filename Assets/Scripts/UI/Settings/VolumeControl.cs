using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Slider))]
public class VolumeControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<UnityEngine.UI.Slider>().onValueChanged.AddListener(ChangeVolume);
    }

    void ChangeVolume(float value)
    {

    }
}

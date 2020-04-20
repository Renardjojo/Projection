using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Slider))]
public class VolumeControl : MonoBehaviour
{
    private UnityEngine.UI.Slider slider = null;

    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<UnityEngine.UI.Slider>();
        slider.value = AudioListener.volume;
        slider.onValueChanged.AddListener(ChangeVolume);

    }

    void ChangeVolume(float value)
    {
        AudioListener.volume = value;
    }
}

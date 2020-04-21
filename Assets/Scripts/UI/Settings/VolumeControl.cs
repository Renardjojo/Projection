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

        float volume = PlayerPrefs.GetFloat("volume", 0.5f);
        slider.value = volume;

        slider.onValueChanged.AddListener(ChangeVolume);
    }

    private void ChangeVolume(float value)
    {
        PlayerPrefs.SetFloat("volume", value);
        AudioListener.volume = value * 2;
    }
}

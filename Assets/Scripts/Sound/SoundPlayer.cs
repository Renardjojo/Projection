using UnityEngine;

public abstract class SoundPlayer : MonoBehaviour
{
    [Header("Sound")]

    [Tooltip("Audio played when the trigger is switched on")]
    [SerializeField]
    protected AudioClip switchedOnSound;

    [Tooltip("Audio played when the trigger is switched off")]
    [SerializeField]
    protected AudioClip switchedOffSound;

    [Tooltip("Whether to use the \"Switched On\" sound for both state changes")]
    [SerializeField]
    protected bool useSameSound;

    [Tooltip("Volume of both sounds")]
    [Range(0f, 1f)]
    [SerializeField]
    protected float volume = 1f;

    protected AudioSource switchedOnAudio;
    protected AudioSource switchedOffAudio;

    protected abstract void PlaySound();
}

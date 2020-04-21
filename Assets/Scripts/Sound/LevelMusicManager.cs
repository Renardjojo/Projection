using UnityEngine;


public class LevelMusicManager : MonoBehaviour
{
    [Header("Level song")]
    [Tooltip("Sound to be played during the level (automatically looped)")]
    [SerializeField] private AudioClip song;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;

    [Header("Effects applied when in shadow")]
    [Tooltip("Frequencies above this one will be progressively dampened")]
    [Range(0f, 22000f)]
    [SerializeField] private float cutoffFrequency = 5000f;

    [Tooltip("Determines how much the filter’s self-resonance is dampened")]
    [Range(1f, 10f)]
    [SerializeField] private float lowpassResonanceQ = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float filterVolume = .5f;

    private new AudioSource audio;
    private AudioLowPassFilter filter;

    void Awake()
    {
        if (song)
        {
            audio      = gameObject.AddComponent<AudioSource>();
            audio.clip = song;
            audio.loop = true;
            audio.volume = volume;

            filter                      = gameObject.AddComponent<AudioLowPassFilter>();
            filter.cutoffFrequency      = cutoffFrequency;
            filter.lowpassResonanceQ    = lowpassResonanceQ;
            filter.enabled              = false;

            audio.Play();

            PlayerController pc = FindObjectOfType<PlayerController>();

            pc.onTransposed     += TurnFilterOn;
            pc.onUntransposed   += TurnFilterOff;
        }
    }
    
    
    private void TurnFilterOn()
    {
        filter.enabled = true;
        audio.volume = filterVolume;
    }

    private void TurnFilterOff()
    {
        filter.enabled = false;
        audio.volume = volume;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer))]
public class CheckPoint : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioClip activationSound = null;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;

    [Header("Configuration")]
    [SerializeField]    private GameObject bodyPlayer                   = null;
    [SerializeField]    private PlayerController playerControllerScript = null;

    [SerializeField]    private float radiusZone = 1f;
                        private bool isActivate = false;

    [SerializeField] private UnityEvent OnCheck = null;

    private AudioSource activationAudio;

    private void Awake()
    {
        if (activationSound)
        {
            activationAudio         = gameObject.AddComponent<AudioSource>();
            activationAudio.clip    = activationSound;
            activationAudio.volume  = volume;

            if (OnCheck == null)
                OnCheck = new UnityEvent();

            OnCheck.AddListener(activationAudio.Play);
        }
    }


    void Update()
    {
        if (!isActivate && (bodyPlayer.transform.position - transform.position).sqrMagnitude < radiusZone * radiusZone)
        {
            isActivate = true;
            playerControllerScript.UseCheckPointPosition(transform.position);
            OnCheck?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        if (!isActivate)
            Gizmos.DrawWireSphere(transform.position, radiusZone);
    }


    public void SetFlag(bool flag)
    {
        isActivate = flag;
    }
}

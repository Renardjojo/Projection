using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct TagObjectEvent
{
    public string tag;
    public UnityEvent OnHit;
}


public class Laser : MonoBehaviour
{
    [Header("Sound")]
    [Tooltip("The constant humming of the laser, whose volume depends on the player distance to the laser")]
    [SerializeField] AudioClip  humming         = null;
    [Tooltip("Range around the laser where the humming is heard")]
    [SerializeField] float      hummingRange    = 5f;

    [Header("Laser configuration")]
    [Tooltip("Can only be changed out of play mode")]
    [SerializeField] float      laserRadius     = 0.2f;
    [Tooltip("Can only be changed out of play mode")]
    [SerializeField] bool       isActivate      = true;
    [SerializeField] float      laserOffSet     = 0.61f;
    [SerializeField] float      maxLaserLenght  = 1000f;

    [SerializeField] List<TagObjectEvent> tagObjectEventList = null;

    private GameObject          laserRay        = null;
    private AudioSource         hummingAudio    = null;
    private PlayerController    pc              = null;
    private bool                playHumming     = true;


    private void Awake()
    {
        laserRay = transform.Find("LaserRay").gameObject;
        GameDebug.AssertInTransform(laserRay != null, transform, "There must be a gameObject named \"LaserRay\" with a CharacterMovements");

        laserRay.transform.localScale = new Vector3(laserRadius, 1f, laserRadius);
        laserRay.SetActive(isActivate);

        if (humming)
        {
            hummingAudio = GetComponent<AudioSource>();

            if (!hummingAudio)          
                hummingAudio = gameObject.AddComponent<AudioSource>();

            hummingAudio.clip = humming;
            hummingAudio.loop = true;
            if (isActivate) hummingAudio.Play();
        }

        pc = GameObject.FindObjectOfType<PlayerController>();

        if (!pc)
            Debug.Log("Not found");
        else
        {
            pc.onTransposed += ToggleHumming;
            pc.onUntransposed += ToggleHumming;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bool containsBodyPlayer = false;

        foreach (TagObjectEvent toe in tagObjectEventList)
        {
            if (toe.tag == "BodyPlayer")
            {
                containsBodyPlayer = true;
                toe.OnHit.AddListener(pc.Kill);
                break;
            }
        }

        if (!containsBodyPlayer)
        {
            tagObjectEventList.Add(new TagObjectEvent() { tag = "BodyPlayer", OnHit = new UnityEvent() });
            tagObjectEventList[tagObjectEventList.Count - 1].OnHit.AddListener(pc.Kill);
        }

        AdjustVolume();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivate)
            return;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        Vector3 startPosition = transform.position + transform.forward * laserOffSet;

        // Bit shift the index of the layer of TransparentFX to get a bit mask
        int layerMask = 1 << LayerMask.NameToLayer("TransparentFX");

        // This would cast rays only against colliders in layer of TransparentFX.
        // But instead we want to collide against everything except layer of TransparentFX. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        if (Physics.Raycast(transform.position + transform.forward * laserOffSet, transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            laserRay.transform.position = startPosition + transform.forward * (hit.distance / 2f);
            laserRay.transform.localScale = new Vector3(laserRadius, hit.distance / 2f / transform.localScale.y, laserRadius);

            for (int i = 0; i < tagObjectEventList.Count; i++)
            {
                if (hit.transform.gameObject.tag == tagObjectEventList[i].tag)
                {
                    tagObjectEventList[i].OnHit?.Invoke();
                    break;
                }
            }
        }
        else
        {
            laserRay.transform.position = startPosition + transform.forward * (maxLaserLenght / 2f);
            laserRay.transform.localScale = new Vector3(laserRadius, maxLaserLenght / 2f / transform.localScale.y, laserRadius);
        }

        AdjustVolume();
    }

    public void setActivate(bool flag)
    {
        if (isActivate != flag)
        {
            isActivate = flag;
            laserRay.SetActive(isActivate);

            if (isActivate && playHumming)
                hummingAudio?.Play();
            else
                hummingAudio?.Stop();
        }
    }

    public void switchState()
    {
        isActivate = !isActivate;
        laserRay.SetActive(isActivate);
    }

    private void ToggleHumming()
    {
        playHumming = !playHumming;
        if (playHumming && isActivate)
        {
            AdjustVolume();
            hummingAudio?.Play();
        }

        else
            hummingAudio?.Stop();
    }


    private void AdjustVolume()
    {
        if (hummingAudio != null && pc.controlledObject)
        {
            float distance = Vector3.Distance(pc.controlledObject.transform.position, transform.position);

            if (distance <= hummingRange)
                hummingAudio.volume = (hummingRange - distance) / hummingRange;

            else
                hummingAudio.volume = 0f;
        }
    }
}

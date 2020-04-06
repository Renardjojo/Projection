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


[ExecuteInEditMode]
public class Laser : MonoBehaviour
{
    [SerializeField, Tooltip("Work on start only")] float  laserRadius = 0.2f;
    [SerializeField, Tooltip("Work on start only")] bool   isActivate  = true;
    [SerializeField] float laserOffSet = 0.61f;
    [SerializeField] float maxLaserLenght = 1000f;

    [SerializeField] List<TagObjectEvent> tagObjectEventList = null;

    GameObject laserRay;


    private void Awake()
    {
        laserRay = transform.Find("LaserRay").gameObject;
        GameDebug.AssertInTransform(laserRay != null, transform, "There must be a gameObject named \"LaserRay\" with a CharacterMovements");

        laserRay.transform.localScale = new Vector3(laserRadius, 1f, laserRadius);
        laserRay.SetActive(isActivate);
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerController pc = GameObject.FindObjectOfType<PlayerController>();

        if (!pc)
            Debug.Log("Not found");

        bool containsBodyPlayer = false;

        foreach (TagObjectEvent toe in tagObjectEventList)
        {
            if (toe.tag == "BodyPlayer")
            {
                containsBodyPlayer = true;
                break;
            }
        }

        if (!containsBodyPlayer)
        {
            tagObjectEventList.Add(new TagObjectEvent() { tag = "BodyPlayer", OnHit = new UnityEvent() });
            tagObjectEventList[tagObjectEventList.Count - 1].OnHit.AddListener(pc.Kill);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivate)
            return;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        Vector3 startPosition = transform.position + transform.forward * laserOffSet;

        if (Physics.Raycast(transform.position + transform.forward * laserOffSet, transform.forward, out hit, Mathf.Infinity))
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
            laserRay.transform.localScale = new Vector3(laserRadius, maxLaserLenght / 2f, laserRadius);
        }
    }

    public void setActivate(bool flag)
    {
        isActivate = flag;
        laserRay.SetActive(isActivate);
    }

    public void switchState()
    {
        isActivate = !isActivate;
        laserRay.SetActive(isActivate);
    }

    private void OnDrawGizmosSelected()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        Vector3 startPosition = transform.position + transform.forward * laserOffSet;

        if (Physics.Raycast(transform.position + transform.forward * laserOffSet, transform.forward, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(startPosition, transform.forward * hit.distance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(startPosition, transform.forward * maxLaserLenght, Color.white);
        }
    }
}

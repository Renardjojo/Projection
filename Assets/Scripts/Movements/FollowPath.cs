﻿using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private GameObject target   = null;
    [SerializeField] private PlayerController pc = null;
    [SerializeField] [Range(0f, 10f)] private float delay = 3f;
    [SerializeField] [Range(1f, 5f)] private float slownessWhenSwitch = 2f;

    private Queue<float>      storedTime     = new Queue<float>();
    private Queue<Vector3>    storedLocation = new Queue<Vector3>();
    private Queue<Quaternion> storedRotation = new Queue<Quaternion>();

    private Vector3 lastLoc;
    //private Quaternion lastRot;
    private float lastTime;

    private float timeModifier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        lastLoc = transform.position;
        //lastRot = transform.rotation;
        lastTime = 0f;

        if (pc)
        {
            pc.onTransposed   += Slow;
            pc.onUntransposed += Reset;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        // add new value
        if (storedLocation.Count == 0
            || target.transform.position != storedLocation.Peek())
            //|| target.transform.rotation != storedRotation.Peek())
        {
            storedLocation.Enqueue(target.transform.position);
            //storedRotation.Enqueue(target.transform.rotation);
            storedTime.Enqueue(Time.time);
        }

        while (storedLocation.Count > 0 && (storedTime.Peek() + delay) * timeModifier <= Time.time)
        {
            lastTime = storedTime.Dequeue();
            lastLoc = storedLocation.Dequeue();
            //lastRot = storedRotation.Dequeue();
        }

        if (storedLocation.Count > 0)
        {
            float next = (storedTime.Peek() + delay) * timeModifier;
            // If the player has a low frame rate, the lerp will set the position better and more accurately.
            if (next - lastTime != 0f)
            {
                float t = (lastTime + delay) * timeModifier;
                transform.position = Vector3.Lerp(lastLoc, storedLocation.Peek(), (Time.time - t) / (next - t));
            }
            //transform.rotation = Quaternion.Lerp(lastRot, storedRotation.Peek(), next - Time.time);
        }
        else
        {
            transform.position = lastLoc;
            //transform.rotation = lastRot;
        }
    }

    private float lTime;

    void Slow()
    {
        // Saving last slowing down time
        lTime = Time.time;

        // NextTime - lTime should be doubled.
        // That is why we multiply the timeModifier.
        timeModifier *= slownessWhenSwitch;
        // However, lTime will also be multiplied timeModifier, and we don't want to modify the previous time.
        // That is why we fix this by adding a delay.
        delay -= lTime / slownessWhenSwitch;
    }

    // Warning : Use it only after using Slow()
    void Reset()
    {                                    
        // First recover the old delay
        delay += lTime / slownessWhenSwitch;
        // Next adding the delay which
        delay += (Time.time - lTime) / timeModifier;

        // Then reset the timeModifier.
        timeModifier = 1f;
    }
}

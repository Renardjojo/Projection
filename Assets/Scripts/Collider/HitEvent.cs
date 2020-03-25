﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[RequireComponent(typeof(Rigidbody))]
public class HitEvent : MonoBehaviour
{
    [SerializeField] private HitEventInfo[] listCollisionEventWithSpecificTag; 

    // Update is called once per frame
    void Update()
    {}

    void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < listCollisionEventWithSpecificTag.Length; i++)
        {
            if (collision.gameObject.tag == listCollisionEventWithSpecificTag[i].collisionWithTag)
            {
                Debug.Log("Hit");
                listCollisionEventWithSpecificTag[i].OnHit?.Invoke();
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[RequireComponent(typeof(Collider))]
public class TriggerHitEvent : MonoBehaviour
{
    [SerializeField] private HitEventInfo[] listCollisionEventWithSpecificTag;
    [SerializeField] public bool isTriggering = true;

    // Update is called once per frame
    void Update()
    {}

    void OnTriggerEnter(Collider collision)
    {
        if (isTriggering)
        {
            for (int i = 0; i < listCollisionEventWithSpecificTag.Length; i++)
            {
                if (collision.gameObject.tag == listCollisionEventWithSpecificTag[i].collisionWithTag)
                {
                    listCollisionEventWithSpecificTag[i].OnHit?.Invoke();
                }
            }
        }
    }
}
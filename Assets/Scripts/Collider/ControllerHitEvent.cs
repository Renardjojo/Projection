using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;


[System.Serializable]
public struct HitEventInfo
{
    //[TagSelector]
    public string collisionWithTag;
    public UnityEvent OnHit;

}

[RequireComponent(typeof(CharacterController))]
public class ControllerHitEvent : MonoBehaviour
{
    [SerializeField] private HitEventInfo[] listCollisionEventWithSpecificTag; 

    // Update is called once per frame
    void Update()
    {}

    void OnControllerColliderHit(ControllerColliderHit collision)
    {
        for (int i = 0; i < listCollisionEventWithSpecificTag.Length; i++)
        {
            if (collision.gameObject.tag == listCollisionEventWithSpecificTag[i].collisionWithTag)
            {
                listCollisionEventWithSpecificTag[i].OnHit?.Invoke();
            }
        }
    }


    void OnCollisionEnter(Collision collision)
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
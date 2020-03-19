﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BodyPlayerCollider : MonoBehaviour
{
    [SerializeField] private UnityEvent OnCollisionHappendWithTagBodyClone = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BodyClone")
        {
            OnCollisionHappendWithTagBodyClone?.Invoke();
        }
    }
}

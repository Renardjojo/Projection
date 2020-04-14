using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLightSub : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            player.ResetShadow();
            player.Transpose();
        }
    }
}

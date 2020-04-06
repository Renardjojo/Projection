using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerShadow : MonoBehaviour
{
    [SerializeField]
    private PlayerController pc = null;

    private void Awake()
    {
        GameDebug.AssertInTransform(pc != null, transform, "pc should not be null");
    }

    // Start is called before the first frame update
    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("ScreenLight"))
        {
            pc.ResetShadow();    
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FollowEntityPositionOnly : MonoBehaviour
{
    [SerializeField] Transform entityPos;
    [SerializeField] Vector3    positionOffSet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = entityPos.position + positionOffSet;    
    }
}

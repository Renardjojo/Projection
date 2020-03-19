using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject                 shadow;
    [SerializeField] private GameObject                 player;
    [SerializeField] private CinemachineVirtualCamera   cameraSetting;

    bool isTransposed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transpose();
        }
    }

    public void moveX(float value)
    {
        transform.position += new Vector3(value, 0, 0);
    }

    public void moveY(float value)
    {
        transform.position += new Vector3(0, value, 0);
    }
    
    public void transpose()
    {
        isTransposed = !isTransposed;

        player.SetActive(!isTransposed);
        cameraSetting.Follow = isTransposed ? shadow.transform : player.transform;
    }
}

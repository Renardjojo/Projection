using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    private GameObject              shadow;
    private Move                    shadowMoveScript;

    private GameObject              body;
    private Move                    bodyMoveScript;

    [SerializeField] private CinemachineVirtualCamera  cameraSetting;

    bool isTransposed = false;

    // Start is called before the first frame update
    void Start()
    {
        shadow              = transform.Find("Shadow").gameObject;
        shadowMoveScript    = shadow.GetComponent<Move>();

        body            = transform.Find("Body").gameObject;
        bodyMoveScript  = body.GetComponent<Move>();

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
        if (isTransposed)
        {
            shadowMoveScript.moveX(value);
        }
        else
        {
            shadowMoveScript    .moveX(value);
            bodyMoveScript      .moveX(value);
        }
    }
    
    public void transpose()
    {
        isTransposed = !isTransposed;
        cameraSetting.Follow = isTransposed ? shadow.transform : body.transform;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    private GameObject              shadow;
    private Move                    shadowMoveScript;
    private Jump                    shadowJumpScript;

    private GameObject              body;
    private Move                    bodyMoveScript;
    private Jump                    bodyJumpScript;

    [SerializeField] private CinemachineVirtualCamera  cameraSetting = null;

    bool isTransposed = false;

    // Start is called before the first frame update
    void Start()
    {
        shadow              = transform.Find("Shadow").gameObject;
        shadowMoveScript    = shadow.GetComponent<Move>();
        shadowJumpScript    = shadow.GetComponent<Jump>();

        body           = transform.Find("Body").gameObject;
        bodyMoveScript = body.GetComponent<Move>();
        bodyJumpScript = body.GetComponent<Jump>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Transpose();
        }
    }

    public void MoveX(float value)
    {
        if (isTransposed)
        {
            shadowMoveScript.MoveX(value);
        }
        else
        {
            shadowMoveScript    .MoveX(value);
            bodyMoveScript      .MoveX(value);
        }
    }

    public void Jump(float value)
    {
        if (isTransposed)
        {
            shadowJumpScript.StartJump(value);
        }
        else
        {
            shadowJumpScript.StartJump(value);
            bodyJumpScript.StartJump(value);
        }
    }
    
    public void Transpose()
    {
        isTransposed = !isTransposed;

        if (isTransposed)
            bodyMoveScript  .MoveX(0f);

        cameraSetting   .Follow = isTransposed ? shadow.transform : body.transform;
    }
}

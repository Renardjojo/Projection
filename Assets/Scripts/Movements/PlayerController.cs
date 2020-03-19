using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    private GameObject              body;
    private Move                    bodyMoveScript;
    private Jump                    bodyJumpScript;

    private GameObject              shadow;

    [SerializeField] private CinemachineVirtualCamera  cameraSetting = null;

    bool isTransposed = false;

    // Start is called before the first frame update
    void Start()
    {
        body           = transform.Find("Body").gameObject;
        bodyMoveScript = body.GetComponent<Move>();
        bodyJumpScript = body.GetComponent<Jump>();

        shadow              = body.transform.Find("Shadow").gameObject;
    }

    // Update is called once per frame
    void Update()
    {}

    public void MoveX(float value)
    {
        if (isTransposed)
        {
            shadow.GetComponent<Move>().MoveX(value);
        }
        else
        {
            bodyMoveScript.MoveX(value);
        }
    }

    public void Jump(float value)
    {
        if (isTransposed)
        {
            shadow.GetComponent<Jump>().StartJump(value);
        }
        else
        {
            bodyJumpScript.StartJump(value);
        }
    }
    
    public void Transpose()
    {
        isTransposed = !isTransposed;

        if (isTransposed)
        {
            bodyMoveScript.MoveX(0f);
            AddComponenetToControlShadow();
        }
        else
        {
            RemoveComponentToUnconstrolShadow();
        }

        cameraSetting   .Follow = isTransposed ? shadow.transform : body.transform;
    }

    private void AddComponenetToControlShadow()
    {
        Rigidbody body = shadow.AddComponent<Rigidbody>();

        body.constraints = RigidbodyConstraints.FreezeRotation;
        body.constraints = RigidbodyConstraints.FreezePositionZ;
        body.useGravity = false;

        shadow.AddComponent<Move>();
        shadow.AddComponent<Jump>();
    }

    private void RemoveComponentToUnconstrolShadow()
    {
        Destroy(shadow.GetComponent<Move>());
        Destroy(shadow.GetComponent<Jump>());
        Destroy(shadow.GetComponent<Rigidbody>());
    }
}

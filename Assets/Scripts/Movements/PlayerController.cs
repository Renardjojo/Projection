using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class PlayerController : MonoBehaviour
{
    private GameObject              body;
    private Move                    bodyMoveScript;
    private Jump                    bodyJumpScript;
    private Rigidbody               bodyRigidbody;

    private GameObject              shadow;
    private Move                    shadowMoveScript;
    private Jump                    shadowJumpScript;
    private Rigidbody               shadowRigidbody;

    private PlayerLight playerLight = null;

    [SerializeField] private CinemachineVirtualCamera  cameraSetting = null;
    [SerializeField] private float                     shadowMaxMovementRadius = 3f;

    bool isTransposed = false;

    public event Action onTransposed;
    public event Action onUntransposed;
    public event Action<Vector3> OnInteractButton;

    // Start is called before the first frame update
    void Start()
    {
        body            = transform.Find("Body").gameObject;
        bodyMoveScript  = body.GetComponent<Move>();
        bodyJumpScript  = body.GetComponent<Jump>();
        bodyRigidbody   = body.GetComponent<Rigidbody>();

        shadow          = transform.Find("Shadow").gameObject;
        shadowMoveScript= shadow.GetComponent<Move>();
        shadowJumpScript= shadow.GetComponent<Jump>();
        shadowRigidbody = shadow.GetComponent<Rigidbody>();
        shadowRigidbody.detectCollisions = false;

        playerLight = body.transform.Find("PlayerLight").gameObject.GetComponent<PlayerLight>();

        Lever[] components = GameObject.FindObjectsOfType<Lever>();
        foreach (Lever lever in components)
        {
            OnInteractButton += lever.TryToSwitch;
        }

        Button[] components2 = GameObject.FindObjectsOfType<Button>();
        foreach (Button button in components2)
        {
            OnInteractButton += button.TryToPress;
        }
    }

    // Update is called once per frame
    void Update()
    {}

    public void MoveX(float value)
    {
        if (isTransposed)
        {
            if ((shadow.transform.position - body.transform.position).magnitude <= shadowMaxMovementRadius)
            {
                shadowMoveScript.MoveX(value);
            }
            else
            {
                Debug.Log("Out");
                shadowRigidbody.isKinematic = true;
            }
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
            //shadowJumpScript.StartJump(value);
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

            onTransposed?.Invoke();
            AddComponenetToControlShadow();
        }
        else
        {
            onUntransposed?.Invoke();
            RemoveComponentToUnconstrolShadow();
        }

        cameraSetting   .Follow = isTransposed ? shadow.transform : body.transform;
    }

    public void Interact()
    {
        OnInteractButton(body.transform.position);
    }

    private void AddComponenetToControlShadow()
    {
        Destroy(shadow.GetComponent<FixedJoint>());
        shadowRigidbody.detectCollisions = true;
        shadowRigidbody.mass = 1f;
    }

    private void RemoveComponentToUnconstrolShadow()
    {
        shadow.AddComponent<FixedJoint>().connectedBody = bodyRigidbody;
        shadowRigidbody.detectCollisions = false;
        shadowRigidbody.velocity = Vector3.zero;
        shadowRigidbody.mass = 0f;
    }



    public void SetLightX(float value)
    {
        playerLight.SetLightX(value);       
    }
    public void SetLightY(float value)
    {
        playerLight.SetLightY(value);
    }
    public void MoveLightX(float value)
    {
        playerLight.MoveLightX(value);
    }
    public void MoveLightY(float value)
    {
        playerLight.MoveLightY(value);
    }
}

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
    [SerializeField] private float  shadowSpeed = 5f;
    //private Move                    shadowMoveScript;
    //private Jump                    shadowJumpScript;
    //private Rigidbody               shadowRigidbody;

    private GameObject playerLight;

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

        shadow          = body.transform.Find("Shadow").gameObject;
        /*shadowMoveScript= shadow.GetComponent<Move>();
        shadowJumpScript= shadow.GetComponent<Jump>();
        shadowRigidbody = shadow.GetComponent<Rigidbody>();
        shadowRigidbody.detectCollisions = false;*/

        playerLight = body.transform.Find("PlayerLight").gameObject;

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
            //shadowMoveScript.MoveX(value);
        }
        else
        {
            bodyMoveScript.MoveX(value);
        }
    }

    public void MoveShadowX(float value)
    {
        Vector3 movement = Vector3.right * value * shadowSpeed * Time.deltaTime;
        if (((shadow.transform.position + movement) - body.transform.position).magnitude <= shadowMaxMovementRadius)
        {
            shadow.transform.position += Vector3.right * value * shadowSpeed * Time.deltaTime;
        }
    }

    public void MoveShadowY(float value)
    {
        Vector3 movement = Vector3.up * value * shadowSpeed * Time.deltaTime;
        if (((shadow.transform.position + movement) - body.transform.position).magnitude <= shadowMaxMovementRadius)
        {
            shadow.transform.position += Vector3.up * value * shadowSpeed * Time.deltaTime;
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
            //bodyMoveScript.MoveX(0f);

            onTransposed?.Invoke();
            //AddComponenetToControlShadow();

            shadow.GetComponent<CapsuleCollider>().isTrigger = false;
        }
        else
        {
            onUntransposed?.Invoke();
            //RemoveComponentToUnconstrolShadow();
            shadow.GetComponent<CapsuleCollider>().isTrigger = true;
        }

        cameraSetting   .Follow = isTransposed ? shadow.transform : body.transform;
    }

    public void Interact()
    {
        OnInteractButton(body.transform.position);
    }
    /*
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
    }*/

    public void SetLightX(float value)
    {
        playerLight.transform.localPosition = new Vector3(value, playerLight.transform.localPosition.y, -0.5f);        
    }
    public void SetLightY(float value)
    {
        playerLight.transform.localPosition = new Vector3(playerLight.transform.localPosition.x, value, -0.5f);
    }
}

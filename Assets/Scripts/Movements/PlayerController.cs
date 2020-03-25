using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class PlayerController : MonoBehaviour
{
    private GameObject              body;
    private CharacterMovements      bodyMoveScript;
    private Rigidbody               bodyRigidbody;

    private GameObject              shadow;
    private CharacterMovements      shadowMoveScript;
    private Rigidbody               shadowRigidbody;

    [SerializeField] private CinemachineVirtualCamera  cameraSetting = null;

    bool isTransposed = false;

    public event Action onTransposed;
    public event Action onUntransposed;
    public event Action<Vector3> OnInteractButton;

    // Start is called before the first frame update
    void Start()
    {
        body            = transform.Find("Body").gameObject;
        bodyMoveScript  = body.GetComponent<CharacterMovements>();
        //bodyRigidbody   = body.GetComponent<Rigidbody>();

        shadow          = body.transform.Find("Shadow").gameObject;
        shadowMoveScript= shadow.GetComponent<CharacterMovements>();
        //shadowRigidbody = shadow.GetComponent<Rigidbody>();
        //shadowRigidbody.detectCollisions = false;

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

        RemoveComponentToUnconstrolShadow();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO : TOREMOVE
        if (Input.GetKeyDown(KeyCode.R))
        {
            Jump();
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
            bodyMoveScript.MoveX(value);
        }
    }

    public void Jump()
    {
        if (isTransposed)
        {
            shadowMoveScript.Jump();
        }
        else
        {
            bodyMoveScript.Jump();
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
        shadowMoveScript.enabled = true;
        //shadowRigidbody.detectCollisions = true;
    }

    private void RemoveComponentToUnconstrolShadow()
    {
        shadowMoveScript.enabled = false;
        //shadowRigidbody.detectCollisions = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using System;

public class PlayerController : MonoBehaviour
{
    private GameObject              body;

    private CharacterMovements      bodyMoveScript;

    private GameObject              shadow;
    private CharacterMovements      shadowMoveScript;

    [SerializeField] private CinemachineVirtualCamera  cameraSetting = null;
    [SerializeField] private TimeManager timeManagerScript;


                        private Vector3 checkPointPosition;
    [SerializeField]    private UnityEvent OnIsDead;

    bool isTransposed = false;

    public event Action onTransposed;
    public event Action onUntransposed;
    public event Action<Vector3> OnInteractButton;

    // Start is called before the first frame update
    void Start()
    {
        body            = transform.Find("Body").gameObject;
        bodyMoveScript  = body.GetComponent<CharacterMovements>();
        GameDebug.AssertInTransform(body != null && bodyMoveScript != null, transform, "There must be a gameObject named \"body\" with a CharacterMovements");

        shadow          = body.transform.Find("Shadow").gameObject;
        shadowMoveScript= shadow.GetComponent<CharacterMovements>();
        GameDebug.AssertInTransform(body != null && bodyMoveScript != null, transform, "There must be a gameObject named \"shadow\" with a CharacterMovements");

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

        checkPointPosition = body.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO : TOREMOVE
        if (Input.GetKeyDown(KeyCode.G))
        {
            TakableBox[] takableBoxes = GameObject.FindObjectsOfType<TakableBox>();

            foreach (TakableBox box in takableBoxes)
            {
                if (box.IsTaken)
                    box.Drop();
                else
                    box.TryToTakeBox(body, 10f);
            }
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

    public void Jump(bool bJump = true)
    {
        if (isTransposed)
        {
            shadowMoveScript.JumpFlag = bJump;
        }
        else
        {
            bodyMoveScript.JumpFlag = bJump;
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
            timeManagerScript.EnableSlowMotionInFirstPlan(true);
        }
        else
        {
            onUntransposed?.Invoke();
            RemoveComponentToUnconstrolShadow();
            timeManagerScript.EnableSlowMotionInFirstPlan(false);
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
    }

    private void RemoveComponentToUnconstrolShadow()
    {
        shadowMoveScript.enabled = false;
    }

    public void Kill()
    {
        body.transform.localPosition = checkPointPosition;
        OnIsDead?.Invoke();
    }

    public void UseCheckPointPosition(Vector3 position)
    {
        checkPointPosition = position;
    }
}

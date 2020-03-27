using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject     body;
    [SerializeField] private GameObject     shadow;
    [SerializeField] private TimeManager    timeManagerScript;
    [SerializeField] private UnityEvent     OnIsDead;

    private CharacterMovements              bodyMoveScript;
    private CharacterMovements              shadowMoveScript;

    public GameObject                       controlledObject { get; private set; }
    private Vector3                         checkPointPosition;
    private bool                            isTransposed;

    public event Action                     onTransposed;
    public event Action                     onUntransposed;
    public event Action<Vector3>            OnInteractButton;

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

        controlledObject = body;

        checkPointPosition = controlledObject.transform.position;
        isTransposed = false;
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
        if (controlledObject == body)
            controlledObject = shadow;
        else
            controlledObject = body;

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
    }

    public void Interact()
    {
        OnInteractButton(controlledObject.transform.position);
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

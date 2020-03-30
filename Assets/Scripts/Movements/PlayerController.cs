using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject     body;
    [SerializeField] private GameObject     shadow;
    [SerializeField] private float          maxShadowDistance;
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
    public event Action<GameObject>         OnInteractCube;

    private Vector3 shadowOffset = 2f * Vector3.forward;

    // Start is called before the first frame update
    void Start()
    {
        body            = transform.Find("Body").gameObject;
        bodyMoveScript  = body.GetComponent<CharacterMovements>();
        GameDebug.AssertInTransform(body != null && bodyMoveScript != null, transform, "There must be a gameObject named \"body\" with a CharacterMovements");

        //shadow          = body.transform.Find("Shadow").gameObject;
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

        TakableBox[] takableBoxes = GameObject.FindObjectsOfType<TakableBox>();
        foreach (TakableBox box in takableBoxes)
        {
            OnInteractCube += box.BoxInteraction;
            if (box.IsTaken)
                box.Drop();
            else
                box.TryToTakeBox(body, 10f);
        }

        RemoveComponentToUnconstrolShadow();

        controlledObject = body;

        checkPointPosition = controlledObject.transform.position;
        isTransposed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransposed)
        {
            shadow.transform.rotation = body.transform.rotation;
            shadow.transform.position = body.transform.position + shadowOffset;

            // So the shadow does not fall through the floor
            if (shadow.transform.position.y < 1f)
                shadow.transform.position = new Vector3(shadow.transform.position.x, 1f, shadow.transform.position.z);
        }
    }

    public void MoveX(float value)
    {
        if (isTransposed)
        {
            if (value != 0f)
            {
                float tmp = Math.Abs(shadow.transform.position.x + value * shadowMoveScript.speedScale * Time.deltaTime - bodyMoveScript.transform.position.x);
                if (tmp < maxShadowDistance)
                    shadowMoveScript.MoveX(value);

                tmp = Math.Abs(shadow.transform.position.x + value * shadowMoveScript.speedScale * Time.deltaTime - bodyMoveScript.transform.position.x);

                if (tmp >= maxShadowDistance)
                    shadowMoveScript.MoveX(-value);
            }

            else
                shadowMoveScript.MoveX(0f);
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
            shadowMoveScript.WallJumpFlag = bJump;
        }
        else
        {
            bodyMoveScript.JumpFlag = bJump;
            bodyMoveScript.WallJumpFlag = bJump;
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
            // To set shadow's velocity to players's
            shadowMoveScript.CopyFrom(bodyMoveScript);

            bodyMoveScript.MoveX(0f);
            onTransposed?.Invoke();
            AddComponenetToControlShadow();
            timeManagerScript.EnableSlowMotionInFirstPlan(true);
        }

        else
        {
            shadowOffset = shadow.transform.position - body.transform.position;

            shadowMoveScript.MoveX(0f);
            onUntransposed?.Invoke();
            RemoveComponentToUnconstrolShadow();
            timeManagerScript.EnableSlowMotionInFirstPlan(false);
        }
    }

    public void Interact()
    {
        if (controlledObject == shadow)
        {
            OnInteractButton(controlledObject.transform.position);
            OnInteractCube(controlledObject);
        }
    }

    public void ResetShadow()
    {
        // Prevent the shadow from being blocked (in light...)
        if (controlledObject == body)
        {
            // Reset shadow location
            shadowOffset = new Vector3(0f, 0f, 2f);
        }
    }

    private void AddComponenetToControlShadow()
    {
        shadow.GetComponent<CapsuleCollider>().enabled = true;
        shadow.GetComponent<CharacterController>().enabled = true;
        shadowMoveScript.enabled = true;
    }

    private void RemoveComponentToUnconstrolShadow()
    {
        shadow.GetComponent<CapsuleCollider>().enabled = false;
        shadow.GetComponent<CharacterController>().enabled = false;
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

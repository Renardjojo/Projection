using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject     body                = null;
    [SerializeField] private GameObject     shadow              = null;
    [SerializeField] private float          maxShadowDistance   = 5f;
    [SerializeField] private TimeManager    timeManagerScript   = null;
    [SerializeField] private UnityEvent     OnIsDead            = null;

    private CharacterMovements              bodyMoveScript;
    private CharacterMovements              shadowMoveScript;

    public GameObject                       controlledObject { get; private set; }
    private Vector3                         checkPointPosition;
    private float                           initialHeight;
    private bool                            isTransposed;
    public  bool                            IsTransposed { get { return isTransposed; } }
    private TakableBox                      currentBox          = null;

    public event Action                     onTransposed;
    public event Action                     onUntransposed;
    public event Action<Vector3>            OnInteractButton;

    private float defaultZOffset = 0f; 
    private Vector3 shadowOffset = 2f * Vector3.forward;

    // Start is called before the first frame update
    void Start()
    {
        body                = transform.Find("Body").gameObject;
        bodyMoveScript      = body.GetComponent<CharacterMovements>();
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

        RemoveComponentToUnconstrolShadow();

        controlledObject    = body;
        checkPointPosition  = controlledObject.transform.position;
        initialHeight       = controlledObject.transform.position.y;
        isTransposed        = false;

        defaultZOffset = shadow.transform.position.z - body.transform.position.z;
        shadowOffset = defaultZOffset * Vector3.forward;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isTransposed)
        {
            shadow.transform.rotation = body.transform.rotation;
            shadow.transform.position = body.transform.position + shadowOffset;

            // So the shadow does not fall through the floor
            if (shadow.transform.position.y < initialHeight)
                shadowOffset.y += initialHeight - shadow.transform.position.y;
        }

        else
        {
            Vector3 bodyToShadow = shadow.transform.position - body.transform.position;
            bodyToShadow[2] = 0f;
            if (bodyToShadow.magnitude > maxShadowDistance)
            {
                bodyToShadow *= maxShadowDistance / bodyToShadow.magnitude - 1f;
                shadow.transform.position += bodyToShadow;
                shadowMoveScript.moveDirection = Vector3.zero;
            }
        }
    }


    public void MoveX(float value)
    {
        if (isTransposed)
            shadowMoveScript.MoveX(value);
        else
            bodyMoveScript.MoveX(value);
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
    
    internal bool IsShadowCollidingWithLightScreen()
    {
        return Physics.Raycast(shadow.transform.position, Vector3.forward, Mathf.Infinity, LayerMask.GetMask("ScreenLight"));
    }

    public void Transpose()
    {
        // Can't tranpose if currently controlling player 
        // when the shadow is in the light screen, since it disappears.
        if (!isTransposed && IsShadowCollidingWithLightScreen())
        {
            return;
        }

        if (currentBox != null)
        {
            DropBox();
        }

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


    public void InteractWithBoxes()
    {
        if (currentBox != null)
        {
            DropBox();
        }
        else
        {
            TakeClosestBox();
        }
    }

    public void DropBox()
    {
        currentBox.Drop(controlledObject.GetComponent<Collider>());
        currentBox = null;
    }

    public void TakeClosestBox()
    {
        TakableBox[] boxes = GameObject.FindObjectsOfType<TakableBox>();
        foreach (TakableBox box in boxes)
        {
            if (box.TryToTakeBox(controlledObject, controlledObject.GetComponent<Collider>()))
            {
                currentBox = box;
                break;
            }
        }
    }

    public void Interact()
    {
        if (controlledObject == shadow)
        {
            OnInteractButton(controlledObject.transform.position);
            InteractWithBoxes();
            //OnInteractCube(controlledObject);
        }
    }

    public void ResetShadow()
    {
        // Prevent the shadow from being blocked (in light...)
        if (controlledObject == body)
        {
            // Reset shadow location
            shadowOffset = new Vector3(0f, 0f, defaultZOffset);
        }
    }

    private void AddComponenetToControlShadow()
    {
        //shadow.GetComponent<CapsuleCollider>().enabled = true;
        shadow.GetComponent<CharacterController>().enabled = true;
        shadowMoveScript.enabled = true;
    }

    private void RemoveComponentToUnconstrolShadow()
    {
        //shadow.GetComponent<CapsuleCollider>().enabled = false;
        shadow.GetComponent<CharacterController>().enabled = false;
        shadowMoveScript.enabled = false;
    }

    public void Kill()
    {
        CharacterController charController = body.GetComponent<CharacterController>();
        charController.enabled = false;
        charController.transform.position = checkPointPosition;
        charController.enabled = true;

        OnIsDead?.Invoke();
    }

    public void UseCheckPointPosition(Vector3 position)
    {
        checkPointPosition = position;
    }
}

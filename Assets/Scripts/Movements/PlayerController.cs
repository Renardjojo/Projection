using UnityEngine;
using UnityEngine.Events;
using System;

/*
[System.Serializable]
struct AudioPlayerComponent
{
    public AudioClip walkSound;
    public AudioClip runSound;
    public AudioClip spawnSound;
    public AudioClip deadSound;
    public AudioClip jumpStartSound;
    public AudioClip jumpEndSound;
    public AudioClip transposeBodyToShadowSound;
    public AudioClip transposeShadowToBodySound;
    public AudioClip interractSound;
}*/

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float          maxShadowDistance   = 5f;
    [SerializeField] private TimeManager    timeManagerScript   = null;
    [SerializeField] private UnityEvent     OnIsDead            = null;
    //[SerializeField] private AudioPlayerComponent audioPlayerComponent;


    private GameObject                      body = null;
    [SerializeField] private CharacterMovementProperties bodyMovementProperties = new CharacterMovementProperties(false);
    private CharacterMovements              bodyMoveScript;
    private Animator                        bodyAnimator;

    private GameObject                      shadow = null;
    [SerializeField] private CharacterMovementProperties shadowMovementProperties = new CharacterMovementProperties(true);
    private CharacterMovements              shadowMoveScript;
    private Animator                        shadowAnimator;

    public GameObject                       controlledObject { get; private set; }
    private Vector3                         checkPointPosition;
    public  bool                            isTransposed { get; private set; }
    private bool                            resetFlag = false;
    private TakableBox                      currentBox          = null;

    public event Action                     onTransposed;
    public event Action                     onUntransposed;
    public event Action<Vector3>            OnInteractButton;

    private float defaultZOffset = 0f; 
    private Vector3 shadowOffset = 2f * Vector3.forward;


    // Start is called before the first frame update
    void Start()
    {

        InitializeBody();
        InitializeShadow();

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
        isTransposed        = false;

        defaultZOffset = shadow.transform.position.z - body.transform.position.z;
        shadowOffset = defaultZOffset * Vector3.forward;
    }

    private void InitializeBody()
    {
        /*Find the body and the character movement script component*/
        body = transform.Find("Body").gameObject;
        bodyMoveScript = body.GetComponent<CharacterMovements>();
        GameDebug.AssertInTransform(body != null && bodyMoveScript != null, transform, "There must be a gameObject named \"Body\" with a CharacterMovements");

        /*Initialize body movement script*/
        bodyMoveScript.properties = bodyMovementProperties;

        if (bodyMovementProperties.avoidSlowMotion)
        {
            float multiplicator = 1f / GameObject.Find("Manager/TimeManager").GetComponent<TimeManager>().getTimeScaleInFirstPlanWhenSwitch();

            bodyMovementProperties.scaleMotion(multiplicator);
        }

        /*Find the animator component*/
        bodyAnimator = body.transform.Find("body").GetComponent<Animator>();
        GameDebug.AssertInTransform(bodyAnimator != null, body.transform, "There must be a gameObject named \"body\" with a Animator");
    }
    
    private void InitializeShadow()
    {
        /*Find the shadow and the character movement script component*/
        shadow = transform.Find("Shadow").gameObject;
        shadowMoveScript = shadow.GetComponent<CharacterMovements>();
        GameDebug.AssertInTransform(shadow != null && shadowMoveScript != null, transform, "There must be a gameObject named \"shadow\" with a CharacterMovements");

        /*Initialize shadow movement script*/
        shadowMoveScript.properties = shadowMovementProperties;

        if (shadowMovementProperties.avoidSlowMotion)
        {
            float multiplicator = 1f / GameObject.Find("Manager/TimeManager").GetComponent<TimeManager>().getTimeScaleInFirstPlanWhenSwitch();

            shadowMovementProperties.scaleMotion(multiplicator);
        }

        /*Find the animator component*/
        shadowAnimator = shadow.transform.Find("body").GetComponent<Animator>();
        GameDebug.AssertInTransform(shadowAnimator != null, shadow.transform, "There must be a gameObject named \"body\" with a Animator");
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isTransposed)
        {
            shadow.transform.rotation = body.transform.rotation;
            shadowMoveScript.DirectMove(body.transform.position + shadowOffset - shadow.transform.position);
            shadowOffset = shadow.transform.position - body.transform.position;
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

        if (resetFlag)
        {
            shadow.transform.position = new Vector3(body.transform.position.x, body.transform.position.y, defaultZOffset);
            shadowOffset = new Vector3(0f, 0f, defaultZOffset);
            resetFlag = false;
        }
    }


    public void MoveX(float value)
    {
        if (isTransposed)
        {
            shadowMoveScript.MoveX(value);
            shadowAnimator.SetFloat("Speed", Mathf.Abs(value));
        }
        else
        {
            bodyMoveScript.MoveX(value);
            bodyAnimator.SetFloat("Speed", Mathf.Abs(value));
            shadowAnimator.SetFloat("Speed", Mathf.Abs(value));
        }
    }


    public void Jump(bool bJump = true)
    {
        if (isTransposed)
        {
            shadowMoveScript.JumpFlag = bJump;
            shadowMoveScript.WallJumpFlag = bJump;

            //shadowAnimator.SetTrigger("Jump");
        }
        else
        {
            bodyMoveScript.JumpFlag = bJump;
            bodyMoveScript.WallJumpFlag = bJump;

            //bodyAnimator.SetTrigger("Jump");
            //shadowAnimator.SetTrigger("Jump");
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
        {
            controlledObject = shadow;
        }
        else
        {
            controlledObject = body;
        }

        isTransposed = !isTransposed;

        if (isTransposed)
        {
            bodyAnimator.SetFloat("Speed", 0f);
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
        resetFlag = true;
    }

    private void AddComponenetToControlShadow()
    {
        //shadow.GetComponent<CapsuleCollider>().enabled = true;
        //shadow.GetComponent<CharacterController>().enabled = true;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("ShadowPlayer"), LayerMask.NameToLayer("Shadow"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("ShadowPlayer"), LayerMask.NameToLayer("ScreenLight"), false);
        shadowMoveScript.enabled = true;
    }

    private void RemoveComponentToUnconstrolShadow()
    {
        //shadow.GetComponent<CapsuleCollider>().enabled = false;
        //shadow.GetComponent<CharacterController>().enabled = false;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("ShadowPlayer"), LayerMask.NameToLayer("Shadow"), true);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("ShadowPlayer"), LayerMask.NameToLayer("ScreenLight"), true);
        shadowMoveScript.enabled = false;
    }

    public void Kill()
    {
        CharacterController charController = bodyMoveScript.controller;
        charController.enabled = false;
        charController.transform.position = checkPointPosition;
        charController.enabled = true;

        charController = shadowMoveScript.controller;
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

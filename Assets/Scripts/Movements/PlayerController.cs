using UnityEngine;
using UnityEngine.Events;
using System;

[System.Serializable]
class EventComponent
{
    public UnityEvent OnIsDead      = null;
    public UnityEvent OnTransposed  = null;
}

[System.Serializable]
class ShadowProperties
{
    public CharacterMovementProperties  movementProperties      = new CharacterMovementProperties(true);
    internal bool                       activateShadow          = true;
    public bool                         activateShadowOnStart   = true;
    public EventComponent               eventComponent;
    public AudioComponent               audioComponent;
}

[System.Serializable]
class BodyProperties
{
    public CharacterMovementProperties  movementProperties = new CharacterMovementProperties(false);
    public EventComponent               eventComponent;
    public AudioComponent               audioComponent;
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float          maxShadowDistance   = 5f;
    [SerializeField] private TimeManager    timeManagerScript   = null;
    [SerializeField] private UnityEvent     OnIsDead            = null;
    [SerializeField] private SpriteRenderer MaxRangeCircleSprite = null;


    private GameObject                      body = null;
    [SerializeField] private BodyProperties bodyProperties;
    private CharacterMovements              bodyMoveScript;
    private Animator                        bodyAnimator;

    private GameObject                      shadow = null;
    [SerializeField] private ShadowProperties shadowProperties;
    private CharacterMovements              shadowMoveScript;
    public Animator                        shadowAnimator { get; private set; }

    public GameObject                       controlledObject { get; private set; }
    private Vector3                         checkPointPosition;
    public  bool                            isTransposed { get; private set; }
    private bool                            resetFlag = false;
    private TakableBox                      currentBox          = null;

    public event Action                     onTransposed;
    public event Action                     onUntransposed;
    public event Action<Vector3>            OnInteractButton;
    public event Action<Vector3>            OnInteractLevelDoor;

    private float defaultZOffset = 0f; 
    private Vector3 shadowOffset = 2f * Vector3.forward;

    private void Awake()
    {
        InitializeShadow();
        InitializeBody();
        
        controlledObject = body;
    }

    // Prevents the animator to be fully accessible by making it public ;
    // We don't want to change the animation in another script, 
    // just wether it is affected by the timescale or not.
    internal void SetShadowAnimatorNormalMode(AnimatorUpdateMode mode)
    {
        shadowAnimator.updateMode = mode;
    }


    // Start is called before the first frame update
    void Start()
    {
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

        LevelDoorController[] components3 = GameObject.FindObjectsOfType<LevelDoorController>();
        foreach (LevelDoorController door in components3)
        {
            OnInteractLevelDoor += door.TryToPress;
        }        

        RemoveComponentToUnconstrolShadow();

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
        bodyMoveScript.properties = bodyProperties.movementProperties;
        bodyMoveScript.audio = bodyProperties.audioComponent;

        /*Find the animator component*/
        bodyAnimator = body.transform.Find("body").GetComponent<Animator>();
        bodyMoveScript.animator = bodyAnimator;
        bodyMoveScript.secondAnimator = shadowAnimator;
        GameDebug.AssertInTransform(bodyAnimator != null, body.transform, "There must be a gameObject named \"body\" with a Animator");

        if (bodyProperties.movementProperties.avoidSlowMotion)
        {
            float multiplicator = 1f / GameObject.Find("Manager/TimeManager").GetComponent<TimeManager>().getTimeScaleInFirstPlanWhenSwitch();

            bodyProperties.movementProperties.scaleMotion(multiplicator);
        }
    }
    
    private void InitializeShadow()
    {
        /*Find the shadow and the character movement script component*/
        shadow = transform.Find("Shadow").gameObject;
        shadowMoveScript = shadow.GetComponent<CharacterMovements>();
        GameDebug.AssertInTransform(shadow != null && shadowMoveScript != null, transform, "There must be a gameObject named \"shadow\" with a CharacterMovements");

        /*Initialize shadow movement script*/
        shadowMoveScript.properties = shadowProperties.movementProperties;
        shadowMoveScript.audio      = shadowProperties.audioComponent;

        /*Find the animator component*/
        shadowAnimator = shadow.transform.Find("body").GetComponent<Animator>();
        shadowMoveScript.animator = shadowAnimator;
        GameDebug.AssertInTransform(shadowAnimator != null, shadow.transform, "There must be a gameObject named \"body\" with a Animator");


        if (shadowProperties.movementProperties.avoidSlowMotion)
        {
            float multiplicator = 1f / GameObject.Find("Manager/TimeManager").GetComponent<TimeManager>().getTimeScaleInFirstPlanWhenSwitch();

            shadowProperties.movementProperties.scaleMotion(multiplicator);
        }

        if (shadowProperties.activateShadowOnStart)
        {
            EnableShadow();
        }
        else
        {
            DisableShadow();
        }
    }

    private void Update()
    {
        Vector3 bodyToShadow = shadow.transform.position - body.transform.position;
        bodyToShadow.z = 0f;
        float bodyToShadowMagnitude = bodyToShadow.magnitude;
        float bodyToShadowMagnitudeDivByMaxDistance = bodyToShadowMagnitude / maxShadowDistance;

        if (bodyToShadowMagnitudeDivByMaxDistance > 1.05f)
        {
            //If the shadow his to far, reset it
            ResetShadow();
        }
        else if (!isTransposed)
        {
            shadow.transform.rotation = body.transform.rotation;
            shadowMoveScript.DirectMove(body.transform.position + shadowOffset - shadow.transform.position);
            shadowOffset = shadow.transform.position - body.transform.position;
        }
        else
        {
            if (bodyToShadowMagnitudeDivByMaxDistance > 0.5f)
            {
                MaxRangeCircleSprite.color = new Color(1f, 1f, 1f, (bodyToShadowMagnitudeDivByMaxDistance > 1f ? 1f : (bodyToShadowMagnitudeDivByMaxDistance - 0.5f) * 2f));
            }
            else
            {
                MaxRangeCircleSprite.color = new Color(1f, 1f, 1f, 0f);
            }

            if (bodyToShadowMagnitude > maxShadowDistance)
            {
                //Found the exedente of distance between the max distance and the actual distance and multiply it by the current vector to get the exedent vector.
                bodyToShadow *= (bodyToShadowMagnitudeDivByMaxDistance) - 1f;

                //Reset the exedent position from the body to the shadow 
                shadowMoveScript.DirectMove(-bodyToShadow);
                shadowMoveScript.moveDirection = Vector3.zero;
            }

            // Remove the player velocity on x to prevent the player from moving/sliding (if on ground)
            // If the player is not on ground, it should have disableInputs = false;
            bodyMoveScript.MoveInput(0f);
        }
    }

    public void FixedUpdate()
    {
        if (resetFlag)
        {
            shadow.transform.position = new Vector3(body.transform.position.x, body.transform.position.y, body.transform.position.z + defaultZOffset);
            shadowOffset = new Vector3(0f, 0f, defaultZOffset);
            resetFlag = false;
            shadowProperties.audioComponent.resetPositionSourceAudio?.Play();
        }

        //Avoid collision bug. If the entity his projected to fast, it is kill
        if(isTransposed && shadowMoveScript.controller.velocity.magnitude > shadowMoveScript.properties.maxVelocity)
        {
            ResetShadow();
            Transpose();
        }
        //Kill the player if he fall in void
        else if (!isTransposed && bodyMoveScript.controller.velocity.magnitude > bodyMoveScript.properties.maxVelocity)
        {
            Kill();
        }
    }

    public void MoveX(float value)
    {
        if (isTransposed)
        {
            shadowMoveScript.MoveInput(value);
            shadowAnimator.SetFloat("Speed", Mathf.Abs(value));
        }
        else
        {
            bodyMoveScript.MoveInput(value);
            bodyAnimator.SetFloat("Speed", Mathf.Abs(value));
            shadowAnimator.SetFloat("Speed", Mathf.Abs(value));
        }
    }


    public void Jump(bool bJump = true)
    {
        if (isTransposed)
        {
            shadowMoveScript.JumpInput();
        }
        else
        {
            bodyMoveScript.JumpInput();
        }
    }
    
    internal bool IsShadowCollidingWithLightScreen()
    {
        if (shadow == null)
            return false;

        return Physics.Raycast(shadow.transform.position, Vector3.forward, Mathf.Infinity, LayerMask.GetMask("ScreenLight"));
    }

    public void Transpose()
    {
        // Can't tranpose if currently controlling player 
        // when the shadow is in the light screen, since it disappears.
        if (!isTransposed && IsShadowCollidingWithLightScreen() ||
            (isTransposed && !body.active) ||
            (!isTransposed && ((shadow && !shadow.active) || (shadowProperties != null && !shadowProperties.activateShadow))))
        {
            return;
        }

        if (currentBox != null)
        {
            DropBox();
        }
        
        controlledObject    = (controlledObject == body) ? shadow : body;
        isTransposed        = !isTransposed;

        if (isTransposed)
        {
            bodyProperties.audioComponent.transposeSourceAudio?.Play();
            bodyAnimator.SetFloat("Speed", 0f);
            // To set shadow's velocity to players's
            shadowMoveScript.CopyFrom(bodyMoveScript);

            //bodyMoveScript.MoveX(0f);
            bodyMoveScript.disableInputs = true;
            onTransposed?.Invoke();
            AddComponenetToControlShadow();
            timeManagerScript.EnableSlowMotionInFirstPlan(true);

            bodyProperties.eventComponent.OnTransposed?.Invoke();
            shadowAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;

            //Reset the shadow animation if the player is on wall. The shadow can't wall jump
            shadowAnimator.Play("Idle", -1, 0f);
            shadowAnimator.SetBool("IsOnWall", false);
            shadowAnimator.SetBool("IsGrounded", false);
            shadowAnimator.SetBool("IsJumping", false);
            shadowMoveScript.isOnWall = false;
            bodyMoveScript.secondAnimator = null;
        }

        else
        {
            shadowProperties.audioComponent.transposeSourceAudio?.Play();
            shadowOffset = shadow.transform.position - body.transform.position;

            shadowMoveScript.MoveInput(0f);
            onUntransposed?.Invoke();
            RemoveComponentToUnconstrolShadow();
            timeManagerScript.EnableSlowMotionInFirstPlan(false);

            shadowAnimator.updateMode = AnimatorUpdateMode.Normal;

            shadowProperties.eventComponent.OnTransposed?.Invoke();
            bodyMoveScript.secondAnimator = shadowAnimator;
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
            shadowProperties.audioComponent.interractSourceAudio?.Play();
            OnInteractButton(controlledObject.transform.position);
            InteractWithBoxes();
        }
        else
        {
            bodyProperties.audioComponent.interractSourceAudio?.Play();
            OnInteractLevelDoor?.Invoke(controlledObject.transform.position);
        }
    }

    public void ResetShadow()
    {
        resetFlag = true;
        shadowMoveScript.moveDirection = Vector3.zero;
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
        bodyProperties.audioComponent.deadSourceAudio?.Play();

        CharacterController charController = bodyMoveScript.controller;
        charController.enabled = false;
        charController.transform.position = checkPointPosition;
        charController.enabled = true;

        charController = shadowMoveScript.controller;
        charController.enabled = false;
        charController.transform.position = checkPointPosition;
        charController.enabled = true;

        bodyMoveScript.moveDirection = Vector3.zero;

        OnIsDead?.Invoke();
        ResetShadow();
    }

    public void UseCheckPointPosition(Vector3 position)
    {
        bodyProperties.audioComponent.spawnSourceAudio?.Play();

        checkPointPosition = position;
    }

    public void EnableShadow()
    {
        if (!shadowProperties.activateShadow)
        {
            shadowProperties.activateShadow = true;
            shadow.transform.Find("body").gameObject.SetActive(true);
        }
    }

    public void DisableShadow ()
    {
        if (shadowProperties.activateShadow)
        {
            if (controlledObject == shadow)
                Transpose();

            shadowProperties.activateShadow = false;
            shadow.transform.Find("body").gameObject.SetActive(false);
            resetFlag = true;
        }
    }

    public void SwitchShadowState()
    {
        shadowProperties.activateShadow = !shadowProperties.activateShadow;

        if (shadowProperties.activateShadow)
        {
            EnableShadow();
        }
        else
        {
            DisableShadow();
        }
    }
}
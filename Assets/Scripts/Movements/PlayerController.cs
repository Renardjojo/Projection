using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEditor.PackageManager.Requests;

[System.Serializable]
class AudioPlayerComponent
{
    public      AudioClip walkingFootSound = null;
    internal    AudioSource walkingFootSoundSourceAudio;
    public      float walkSoundDelay = 1f;
    public      float walkSoundOffSet = 0f;
    internal    float walkDelay = 0f;

    public      AudioClip spawnSound = null;
    internal    AudioSource spawnSourceAudio;

    public      AudioClip deadSound = null;
    internal    AudioSource deadSourceAudio;

    public      AudioClip jumpStartSound = null;
    internal    AudioSource jumpStartSourceAudio;

   /* public      AudioClip jumpEndSound = null;
    internal    AudioSource jumpEndSourceAudio;*/

    public      AudioClip transposeBodyToShadowSound = null;
    internal    AudioSource transposeBodyToShadowSourceAudio;

    public      AudioClip transposeShadowToBodySound = null;
    internal    AudioSource transposeShadowToBodySourceAudio;

    public      AudioClip interractSound = null;
    internal    AudioSource interractSourceAudio;

}


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
}

[System.Serializable]
class BodyProperties
{
    public CharacterMovementProperties movementProperties = new CharacterMovementProperties(false);
    public EventComponent eventComponent;
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float          maxShadowDistance   = 5f;
    [SerializeField] private TimeManager    timeManagerScript   = null;
    [SerializeField] private UnityEvent     OnIsDead            = null;
    [SerializeField] private AudioPlayerComponent audioPlayerComponent;


    private GameObject                      body = null;
    [SerializeField] private BodyProperties bodyProperties;
    private CharacterMovements              bodyMoveScript;
    private Animator                        bodyAnimator;

    private GameObject                      shadow = null;
    [SerializeField] private ShadowProperties shadowProperties;
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
    public event Action<Vector3>            OnInteractLevelDoor;

    private float defaultZOffset = 0f; 
    private Vector3 shadowOffset = 2f * Vector3.forward;

    private void Awake()
    {
        initializeSoundComponent();
    }

    void initializeSoundComponent()
    {
        if (audioPlayerComponent.walkingFootSound != null)
        {
            audioPlayerComponent.walkingFootSoundSourceAudio = gameObject.AddComponent<AudioSource>();
            audioPlayerComponent.walkingFootSoundSourceAudio.clip = audioPlayerComponent.walkingFootSound;

            audioPlayerComponent.walkDelay = audioPlayerComponent.walkSoundOffSet % audioPlayerComponent.walkSoundDelay;
        }

        if (audioPlayerComponent.spawnSound != null)
        {
            audioPlayerComponent.spawnSourceAudio = gameObject.AddComponent<AudioSource>();
            audioPlayerComponent.spawnSourceAudio.clip = audioPlayerComponent.spawnSound;
        }

        if (audioPlayerComponent.deadSound != null)
        {
            audioPlayerComponent.deadSourceAudio = gameObject.AddComponent<AudioSource>();
            audioPlayerComponent.deadSourceAudio.clip = audioPlayerComponent.deadSound;
        }

        if (audioPlayerComponent.jumpStartSound != null)
        {
            audioPlayerComponent.jumpStartSourceAudio = gameObject.AddComponent<AudioSource>();
            audioPlayerComponent.jumpStartSourceAudio.clip = audioPlayerComponent.jumpStartSound;
        }

        /*
        if (audioPlayerComponent.jumpEndSound != null)
        {
            audioPlayerComponent.jumpEndSourceAudio = gameObject.AddComponent<AudioSource>();
            audioPlayerComponent.jumpEndSourceAudio.clip = audioPlayerComponent.jumpEndSound;
        }*/

        if (audioPlayerComponent.transposeBodyToShadowSound != null)
        {
            audioPlayerComponent.transposeBodyToShadowSourceAudio = gameObject.AddComponent<AudioSource>();
            audioPlayerComponent.transposeBodyToShadowSourceAudio.clip = audioPlayerComponent.transposeBodyToShadowSound;
        }

        if (audioPlayerComponent.transposeShadowToBodySound != null)
        {
            audioPlayerComponent.transposeShadowToBodySourceAudio = gameObject.AddComponent<AudioSource>();
            audioPlayerComponent.transposeShadowToBodySourceAudio.clip = audioPlayerComponent.transposeShadowToBodySound;
        }

        if (audioPlayerComponent.interractSound != null)
        {
            audioPlayerComponent.interractSourceAudio = gameObject.AddComponent<AudioSource>();
            audioPlayerComponent.interractSourceAudio.clip = audioPlayerComponent.interractSound;
        }
    }

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

        LevelDoorController[] components3 = GameObject.FindObjectsOfType<LevelDoorController>();
        foreach (LevelDoorController door in components3)
        {
            OnInteractLevelDoor += door.TryToPress;
        }
        

        RemoveComponentToUnconstrolShadow();

        controlledObject    = body;
        checkPointPosition  = controlledObject.transform.position;
        isTransposed        = false;

        defaultZOffset = shadow.transform.position.z - body.transform.position.z;
        shadowOffset = defaultZOffset * Vector3.forward;

        audioPlayerComponent.spawnSourceAudio?.Play();
    }

    private void InitializeBody()
    {
        /*Find the body and the character movement script component*/
        body = transform.Find("Body").gameObject;
        bodyMoveScript = body.GetComponent<CharacterMovements>();
        GameDebug.AssertInTransform(body != null && bodyMoveScript != null, transform, "There must be a gameObject named \"Body\" with a CharacterMovements");

        /*Initialize body movement script*/
        bodyMoveScript.properties = bodyProperties.movementProperties;

        /*Find the animator component*/
        bodyAnimator = body.transform.Find("body").GetComponent<Animator>();
        GameDebug.AssertInTransform(bodyAnimator != null, body.transform, "There must be a gameObject named \"body\" with a Animator");

        if (bodyProperties.movementProperties.avoidSlowMotion)
        {
            float multiplicator = 1f / GameObject.Find("Manager/TimeManager").GetComponent<TimeManager>().getTimeScaleInFirstPlanWhenSwitch();

            bodyProperties.movementProperties.scaleMotion(multiplicator);
            bodyAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
        else
        {
            bodyAnimator.updateMode = AnimatorUpdateMode.Normal;
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

        /*Find the animator component*/
        shadowAnimator = shadow.transform.Find("body").GetComponent<Animator>();
        GameDebug.AssertInTransform(shadowAnimator != null, shadow.transform, "There must be a gameObject named \"body\" with a Animator");


        if (shadowProperties.movementProperties.avoidSlowMotion)
        {
            float multiplicator = 1f / GameObject.Find("Manager/TimeManager").GetComponent<TimeManager>().getTimeScaleInFirstPlanWhenSwitch();

            shadowProperties.movementProperties.scaleMotion(multiplicator);
            shadowAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
        else
        {
            shadowAnimator.updateMode = AnimatorUpdateMode.Normal;
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

            // Remove the player velocity on x to prevent the player from moving/sliding (if on ground)
            // If the player is not on ground, it should have disableInputs = false;
            bodyMoveScript.MoveX(0f); 
        }

        if (resetFlag)
        {
            shadow.transform.position = new Vector3(body.transform.position.x, body.transform.position.y, body.transform.position.z + defaultZOffset);
            shadowOffset = new Vector3(0f, 0f, defaultZOffset);
            resetFlag = false;
        }
    }


    public void MoveX(float value)
    {
        if (value != 0f)
        {
            audioPlayerComponent.walkDelay += Time.deltaTime * Time.timeScale;

            if (audioPlayerComponent.walkDelay >= (audioPlayerComponent.walkSoundDelay * (Mathf.Abs(Mathf.Abs(value) - 1f) + 1f)))
            {
                audioPlayerComponent.walkDelay = 0f;
                audioPlayerComponent.walkingFootSoundSourceAudio?.Play();
            }
        }

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
            audioPlayerComponent.jumpStartSourceAudio?.Play();

            shadowMoveScript.Jump();
        }
        else
        {
            audioPlayerComponent.jumpStartSourceAudio?.Play();

            bodyMoveScript.Jump();
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
            audioPlayerComponent.transposeBodyToShadowSourceAudio?.Play();
            bodyAnimator.SetFloat("Speed", 0f);
            // To set shadow's velocity to players's
            shadowMoveScript.CopyFrom(bodyMoveScript);

            //bodyMoveScript.MoveX(0f);
            bodyMoveScript.disableInputs = true;
            onTransposed?.Invoke();
            AddComponenetToControlShadow();
            timeManagerScript.EnableSlowMotionInFirstPlan(true);

            shadowProperties.eventComponent.OnTransposed?.Invoke();
        }

        else
        {
            audioPlayerComponent.transposeShadowToBodySourceAudio?.Play();
            shadowOffset = shadow.transform.position - body.transform.position;

            shadowMoveScript.MoveX(0f);
            onUntransposed?.Invoke();
            RemoveComponentToUnconstrolShadow();
            timeManagerScript.EnableSlowMotionInFirstPlan(false);

            bodyAnimator.Play("Idle", -1, 0f);
            shadowAnimator.Play("Idle", -1, 0f);

            bodyProperties.eventComponent.OnTransposed?.Invoke();
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
            audioPlayerComponent.interractSourceAudio?.Play();
            OnInteractButton(controlledObject.transform.position);
            InteractWithBoxes();
        }
        else
        {
            OnInteractLevelDoor?.Invoke(controlledObject.transform.position);
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
        audioPlayerComponent.deadSourceAudio?.Play();

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
        audioPlayerComponent.spawnSourceAudio?.Play();

        checkPointPosition = position;
    }

    public void EnableShadow ()
    {
        shadowProperties.activateShadow = true;
        shadow.transform.Find("body").gameObject.SetActive(true);
        resetFlag = true;
    }

    public void DisableShadow ()
    {
        if (controlledObject == shadow)
            Transpose();

        shadowProperties.activateShadow = false;
        shadow.transform.Find("body").gameObject.SetActive(false);
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

using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField, Range(0f, 1f), Tooltip("0 = Time paused. 1 = Normal time")]
    private float timeScaleInFirstPlanWhenSwitch = 0.5f;

    bool isActivate = false;
    private float fixedDeltaTime;

    // Start is called before the first frame update
    void Awake()
    {
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {}

    public void EnableSlowMotionInFirstPlan(bool value)
    {
        if (value)
        {
            Time.timeScale = timeScaleInFirstPlanWhenSwitch;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
    }

    public void SwitchSlowMotionInFirstPlan()
    {
        if (isActivate)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
        else
        {
            Time.timeScale = timeScaleInFirstPlanWhenSwitch;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
        isActivate = !isActivate;
    }

    public float getTimeScaleInFirstPlanWhenSwitch() { return timeScaleInFirstPlanWhenSwitch;  }
}

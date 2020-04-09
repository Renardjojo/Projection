using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PhysicLever : Trigger
{
    /*Ex version with gravity
    [SerializeField]
    [Range(0f, 10f)]
    [Tooltip("Damper represent the force that the player must use to move the lever. 0 if no dumper and 10 for hard lever to move")]
    protected float         leverDamper = 2f;

    [SerializeField]
    [Range(-30f, 30f)]
    [Tooltip("Angle to activate swhitch the state of the lever. By default 0 if the lever is activate when it is on the middle position")]
    protected float activateAngle = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        hingeJoint = transform.Find("handle").GetComponent<HingeJoint>();

        Affect new Spring setting to the HingeJoint
        JointSpring js = new JointSpring();
        js.damper = leverDamper;
        hingeJoint.spring = js;
    }
    */
}

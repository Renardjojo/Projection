using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicLevelSub : MonoBehaviour
{
    [SerializeField]
    private PhysicLever lever = null;
    [SerializeField]
    private GameObject rotatedObject = null;

    Vector2 up = - Vector2.up;

    // Start is called before the first frame update
    void Start()
    {
        //if (!lever)
        //{
        //    string objectHierarchy = "";
        //    GameObject o = this.gameObject;

        //    //while (o.transform.parent != null)
        //    //{
        //    //    objectHierarchy += o.name + " <- ";
        //    //}
        //    //Debug.LogError("Lever should not be null in " + objectHierarchy);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterController charController = other.gameObject.GetComponent<CharacterController>();
        if (charController)
        {
            float dot = 2 * Vector3.Dot(charController.velocity, new Vector2(-up.y, up.x));
            rotatedObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, dot < 0 ? 15 : -15));

            lever.SwitchLeverState();
        }
    }
}

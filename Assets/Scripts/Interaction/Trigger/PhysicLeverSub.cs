using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicLeverSub : MonoBehaviour
{
    [SerializeField]
    private PhysicLever lever = null;
    [SerializeField]
    private GameObject rotatedObject = null;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {

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
            float dot = 2 * Vector3.Dot(charController.velocity, transform.right /* normal of the lever direction, so normal of transform.up, and since we are in 2D, it is the transform.up = transform.right */);
            Debug.Log(dot);
            rotatedObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, dot < 0 ? 15 : -15));

            if (dot < 0)
                lever.Enable();
            else
                lever.Disable();
        }
    }
}

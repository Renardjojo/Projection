using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void deleg();

    [SerializeField]
    public class Inputs : Dictionary<KeyCode, deleg> { }

    [SerializeField]
    private Inputs truc;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("space key was pressed");
        }
    }
}

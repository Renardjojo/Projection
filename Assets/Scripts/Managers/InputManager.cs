using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    //public delegate void deleg();

    //[System.Serializable]
    //public class Inputs : Dictionary<KeyCode, UnityEvent> { }

    //[SerializeField]
    //private Inputs truc;

    [SerializeField]
    private UnityEvent OnEscapeIsClick;

    [SerializeField]
    private UnityEvent<float> OnAxisX;

    [SerializeField]
    private UnityEvent<float> OnAxisY;

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnEscapeIsClick?.Invoke();
        }

        if (Input.GetAxis("Horizontal") != 0f)
        {
            OnAxisX?.Invoke(Input.GetAxis("Horizontal"));
        }

        if (Input.GetAxis("Vertical") != 0f)
        {
            OnAxisY?.Invoke(Input.GetAxis("Vertical"));
        }
    }
}

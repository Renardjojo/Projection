using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
struct InputPair
{
    public KeyCode keycode;
    public UnityEvent e;
}

public class InputManager : MonoBehaviour
{
    public delegate void deleg();

    //[SerializeField]
    //public class Inputs : Dictionary<KeyCode, deleg> { }

    [SerializeField]
    private List<InputPair> inputList;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (InputPair input in inputList)
        {
            if (Input.GetKeyDown(input.keycode))
            {
                input.e?.Invoke();
            }
        }
    }
}

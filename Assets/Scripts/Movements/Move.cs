using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveX(float value)
    {
        transform.position += new Vector3(value, 0, 0);
    }

    public void moveY(float value)
    {
        transform.position += new Vector3(0, value, 0);
    }

    public void onQPressed()
    {
        Debug.Log("pressed");
    }

    public void onQReleased()
    {
        Debug.Log("released");
    }
}

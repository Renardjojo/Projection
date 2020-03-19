using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float delay;

    private Queue<float>      storedTime     = new Queue<float>();
    private Queue<Vector3>    storedLocation = new Queue<Vector3>();
    private Queue<Quaternion> storedRotation = new Queue<Quaternion>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // add new value
        if (storedLocation.Count == 0
            || target.transform.position != storedLocation.Peek()
            || target.transform.rotation != storedRotation.Peek())
        {
            storedLocation.Enqueue(target.transform.position);
            storedRotation.Enqueue(target.transform.rotation);
            storedTime.Enqueue(Time.time + delay);
        }

        while (storedLocation.Count > 0 && storedTime.Peek() <= Time.time)
        {
            storedTime.Dequeue();
            transform.position = storedLocation.Dequeue();
            transform.rotation = storedRotation.Dequeue();
        }
    }

    private void FixedUpdate()
    {

    }
}

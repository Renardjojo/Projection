using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] [Range(0f, 10f)]private float delay = 3f;

    private Queue<float>      storedTime     = new Queue<float>();
    private Queue<Vector3>    storedLocation = new Queue<Vector3>();
    private Queue<Quaternion> storedRotation = new Queue<Quaternion>();

    private Vector3 lastLoc;
    //private Quaternion lastRot;
    private float lastTime;

    // Start is called before the first frame update
    void Start()
    {
        lastLoc = transform.position;
        //lastRot = transform.rotation;
        lastTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        // add new value
        if (storedLocation.Count == 0
            || target.transform.position != storedLocation.Peek())
            //|| target.transform.rotation != storedRotation.Peek())
        {
            storedLocation.Enqueue(target.transform.position);
            //storedRotation.Enqueue(target.transform.rotation);
            storedTime.Enqueue(Time.time + delay);
        }

        while (storedLocation.Count > 0 && storedTime.Peek() <= Time.time)
        {
            lastTime = storedTime.Dequeue();
            lastLoc = storedLocation.Dequeue();
            //lastRot = storedRotation.Dequeue();
        }

        if (storedLocation.Count > 0)
        {
            float next = storedTime.Peek();
            // If the player has a low frame rate, the lerp will set the position better and more accurately.
            transform.position = Vector3.Lerp(lastLoc, storedLocation.Peek(), (Time.time - lastTime) / (next - lastTime));
            //transform.rotation = Quaternion.Lerp(lastRot, storedRotation.Peek(), next - Time.time);
        }
        else
        {
            transform.position = lastLoc;
            //transform.rotation = lastRot;
        }
    }
}

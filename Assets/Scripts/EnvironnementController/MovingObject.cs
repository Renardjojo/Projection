using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
   
    [SerializeField] Vector3    startPosition;
    [SerializeField] bool       useCurrentPoisitionToStart = true;
    [SerializeField] Vector3    endPosition;
    [SerializeField] bool       moveTowardEndPosition = true; //false if move toward sart position

    [SerializeField]
    [Range(0f, 1f)]
    [Tooltip("Define the platform spedd between 0 and 1 by second")]
    private float moveSpeed = 0.5f;

    float step = 0f; //bewteen 0 and 1. 0 on start position and 1 on end position
    [SerializeField] bool isMoving = true;


    // Start is called before the first frame update
    void Start()
    {
        if (useCurrentPoisitionToStart)
        {
            startPosition = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (moveTowardEndPosition)
            {
                step += moveSpeed * Time.deltaTime;
                if (step > 1f)
                {
                    step = 1f;
                    moveTowardEndPosition = false;
                }
            }
            else
            {
                step -= moveSpeed * Time.deltaTime;
                if (step < 0f)
                {
                    step = 0f;
                    moveTowardEndPosition = true;
                }
            }

            transform.position = startPosition + (endPosition - startPosition) * step;
        }
    }

    public void EnableMovement(bool value)
    {
        isMoving = value;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(useCurrentPoisitionToStart ? transform.position : startPosition, endPosition);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovingObject : MonoBehaviour
{
   
    [SerializeField] Vector3    startPosition;
    [SerializeField] bool       useCurrentPoisitionToStart = true;
    [SerializeField] Vector3    endPosition;
    [SerializeField] bool       endPositionDependingOfStartPosition = false;
    [SerializeField] bool       moveTowardEndPosition = true; //false if move toward sart position
    [SerializeField] bool       backAndForth = true;

    [SerializeField]
    [Range(0f, 1f)]
    [Tooltip("Define the platform spedd between 0 and 1 by second")]
    private float moveSpeed = 0.5f;
    [SerializeField] bool useTimeScale = false;
    internal Vector3 frameDisplacement;

    float step = 0f; //bewteen 0 and 1. 0 on start position and 1 on end position
    [SerializeField] bool isMoving = true;


    // Start is called before the first frame update
    void Start()
    {
        if (useCurrentPoisitionToStart)
        {
            startPosition = transform.position;
        }

        if (endPositionDependingOfStartPosition)
        {
            endPosition += startPosition;
        }

        if(moveTowardEndPosition)
        {
            step = 0f;
        }
        else
        {
            step = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float stepExFram = step;
        if (isMoving)
        {
            if (moveTowardEndPosition)
            {
                step += moveSpeed * Time.unscaledDeltaTime * (useTimeScale ? Time.timeScale : 1f);

                if (step >= 1f)
                {
                    step = 1f;
                    moveTowardEndPosition = false;
                    if (!backAndForth)
                    {
                        isMoving = false;
                    }
                }
            }
            else
            {
                step -= moveSpeed * Time.unscaledDeltaTime * (useTimeScale ? Time.timeScale : 1f);
                if (step <= 0f)
                {
                    step = 0f;
                    moveTowardEndPosition = true;
                    if (!backAndForth)
                    {
                        isMoving = false;
                    }
                }
            }

            frameDisplacement = (endPosition - startPosition) * (step - stepExFram);
            transform.position += frameDisplacement;
        }
        else
        {
            frameDisplacement = Vector3.zero;
        }
    }

    public void EnableMovement(bool value)
    {
        isMoving = value;
    }

    public void ToggleMovementState()
    {
        isMoving = !isMoving;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 start = startPosition;
        if (useCurrentPoisitionToStart)
        {
            start = transform.position;
        }

        Vector3 end = endPosition;
        if (endPositionDependingOfStartPosition)
        {
            end += startPosition;
        }

        Gizmos.DrawLine(start, end);
    }

    public void ResetPosition(bool _isMoving)
    {
        transform.position = startPosition;
        step = 0f;
        moveTowardEndPosition = true;
        isMoving = _isMoving;
    }
}

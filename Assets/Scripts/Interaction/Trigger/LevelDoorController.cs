using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelDoorController : Trigger
{
    [SerializeField] private float  interactionRadius = 2f;
    private float                   interactionRadius2;
    [SerializeField] UnityEvent OnPressEvent;
    [SerializeField, Tooltip("If door is End Door. This Door will activate an boolean with the name of the current scene to save that the player finish the level")]
    bool isEndDoor = false;

    private void Start()
    {
        interactionRadius2 = interactionRadius * interactionRadius;
    }

    public void Press()
    {
        IsOn = true;

        if (isEndDoor)
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 1);
        }

        OnPressEvent?.Invoke();
    }

    public void TryToPress(Vector3 playerPos)
    {
        if ((playerPos - transform.position).sqrMagnitude < interactionRadius2)
        {
            Press();
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}

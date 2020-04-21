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

    private void Start()
    {
        interactionRadius2 = interactionRadius * interactionRadius;
    }

    public void Press()
    {
        if (IsOn)
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 1);
            OnPressEvent?.Invoke();
        }
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

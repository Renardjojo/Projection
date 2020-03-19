using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollider : MonoBehaviour
{
    private GameObject              shadow;
    private GameObject              body;

    [SerializeField] private UnityEvent OnCollisionHappendWithTagBodyClone;
    [SerializeField] private UnityEvent OnCollisionHappendWithTagShadowClone;

    // Start is called before the first frame update
    void Start()
    {
        shadow   = transform.Find("Shadow").gameObject;
        body     = transform.Find("Body").gameObject;
    }

    public void collisionEnterTagBodyClone()
    {
        OnCollisionHappendWithTagBodyClone?.Invoke();
    }

    public void collisionEnterTagShadowClone()
    {
        OnCollisionHappendWithTagShadowClone?.Invoke();
    }
}

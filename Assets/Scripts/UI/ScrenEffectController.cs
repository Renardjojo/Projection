using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrenEffectController : MonoBehaviour
{

    [SerializeField] Animator transposeEffectAnimator;

    // Start is called before the first frame update
    void Awake()
    {}

    // Update is called once per frame
    void Update()
    {}

    public void EnableTransposedEffect(bool state)
    {
        transposeEffectAnimator.SetBool("IsTransposed", state);
    }
}

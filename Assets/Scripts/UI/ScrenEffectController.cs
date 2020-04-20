using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrenEffectController : MonoBehaviour
{
    [SerializeField] Animator transposeEffectAnimator;

    public void EnableTransposedEffect(bool state)
    {
        transposeEffectAnimator.SetBool("IsTransposed", state);
    }
}

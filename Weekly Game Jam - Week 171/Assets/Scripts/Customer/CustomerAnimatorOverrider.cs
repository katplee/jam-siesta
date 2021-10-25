using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerAnimatorOverrider : MonoBehaviour
{
    private Animator animator = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimations(AnimatorOverrideController overrideController)
    {
        animator.runtimeAnimatorController = overrideController;
    }
}

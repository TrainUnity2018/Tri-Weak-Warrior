using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoSingleton<PlayerAnimationController>
{

    public Animator animator;

    void Start()
    {

    }

    void Update()
    {

    }

    public void Flip(bool direction) //true for left, false for right
    {
        Vector3 theScale = transform.localScale;
        if (direction)
            theScale.x = -1;
        else
            theScale.x = 1;
        transform.localScale = theScale;
    }

    public void SetMovementState(int state)
    {
        animator.SetInteger("MovementState", state);
    }

    public void SetArmorState(int state)
    {
        animator.SetInteger("ArmorState", state);
    }
}

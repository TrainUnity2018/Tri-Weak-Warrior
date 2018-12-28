using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour {

    public Animator animator;

    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

    }

    public virtual void Slash(bool direction)
    {
        Flip(direction);
        SetMovementState((int)PlayerStateControl.MovementState.Slash);
    }

    public virtual void Flip(bool direction)
    {
        Vector3 theScale = transform.localScale;
        if (direction)
            theScale.x = -1;
        else
            theScale.x = 1;
        transform.localScale = theScale;
    }

    public virtual void SetMovementState(int state)
    {
        animator.SetInteger("MovementState", state);
    }

    public virtual void SetArmorState(int state)
    {
        animator.SetInteger("ArmorState", state);
    }
}

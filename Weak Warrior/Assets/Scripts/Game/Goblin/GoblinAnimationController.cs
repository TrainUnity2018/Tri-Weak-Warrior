using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAnimationController : MonsterAnimationController {

	public Goblin actor;
	public Animator animator;
	public SpriteRenderer sprite;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void Flip(bool direction)
	{
        Vector3 theScale = transform.localScale;
        if (direction)
            theScale.x = transform.localScale.x;
        else
            theScale.x = -transform.localScale.x;
        transform.localScale = theScale;
	}

	public override void SetMovementState(int state)
	{
		animator.SetInteger("MovementState", state);
	}

	public override void Disable()
	{
		sprite.enabled = false;
		animator.enabled = false;
	}

	public virtual void AttackEnd()
	{
		actor.AttackEnd();
	}

    public virtual void EnableDamageBox()
    {
        actor.EnableDamageBox();
    }

    public virtual void DisableDamageBox()
    {
        actor.DisableDamageBox();
    }

    public virtual void EnableHitBox()
    {
        actor.EnableHitBox();
    }

    public virtual void DisableHitBox()
    {
        actor.DisableHitBox();
    }
}

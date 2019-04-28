using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void Setup(bool direction) //true for left, false for right
    {

    }
	
	public virtual void Walk()
	{

	}

	public virtual void Attack()
	{

	}

	public virtual void AttackDelayTiming() 
	{

	}

	public virtual void AttackEnd()
	{

	}

	public virtual void TakeDamage(int playersMovementSate)
	{

	}

	public virtual void OnDead()
	{

	}

	public virtual void Pause()
	{

	}

	public virtual void UnPause()
	{

	}

	public virtual void EnableDamageBox()
	{

	}

	public virtual void DisableDamageBox()
	{

	}

	public virtual void EnableHitBox()
	{

	}

	public virtual void DisableHitBox()
	{

	}
}

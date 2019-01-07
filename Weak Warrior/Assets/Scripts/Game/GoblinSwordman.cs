using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSwordman : MonoBehaviour {

    public float moveSpeed;
    public int damage;
    public bool spawnDirection;

    public Animator animator;

    public GoblinSwordman_DamageBox damageBox;
    public BoxCollider2D hitBox;

    public enum MovementState
    {
        Walk,
        Slash
    }

    public int currentState;

    // Use this for initialization
    void Start () {
        currentState = (int)MovementState.Walk;
        animator.SetInteger("State", currentState);
	}
	
	// Update is called once per frame
	void Update () {
        Walk(spawnDirection);
	}

    void OnTriggerEnter2D(Collider2D col)
    {

    }

    public virtual void Walk(bool spawnDirection)
    {
        if (currentState == (int)MovementState.Walk)
        {
            if (spawnDirection == true)
            {
                gameObject.transform.position += new Vector3(moveSpeed, 0) * Time.deltaTime;
            }
            else
            {
                gameObject.transform.position -= new Vector3(moveSpeed, 0) * Time.deltaTime;
            }
        }
    }

    public virtual void Slash()
    {
        if (currentState == (int)MovementState.Walk)
        {
            currentState = (int)MovementState.Slash;
            animator.SetInteger("State", currentState);
        }
    }

    public virtual void EnableHitBox()
    {
        hitBox.enabled = true;
    }

    public virtual void DisableHitBox()
    {
        hitBox.enabled = false;
    }

    public virtual void EnableDamageBox()
    {
        damageBox.EnableDamageBox();
    }

    public virtual void DisableDamageBox()
    {
        damageBox.DisableDamageBox();
    }

    public virtual void OnCollide(Collider2D col)
    {

    }
}

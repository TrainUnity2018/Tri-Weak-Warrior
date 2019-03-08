using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSwordman : MonoBehaviour {

    public float moveSpeed;
    public int damage;
    public bool spawnDirection;

    public float slashDelay;
    protected float slashDelayTimer;

    public Animator animator;

    public GoblinSwordman_DamageBox damageBox;
    public BoxCollider2D hitBox;

    public bool pause;

    public enum MovementState
    {
        Walk,
        Slash,
        SlashDelay,
    }

    public int currentMovementState;

    // Use this for initialization
    void Start () {
        currentMovementState = (int)MovementState.Walk;
        animator.SetInteger("State", currentMovementState);
        pause = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!pause)
        {
            Walk();
            SlashDelayTiming();
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        OnCollide(col);
    }

    public virtual void Setup(bool direction, ModelLevel model = null)
    {
        Flip(direction);
        spawnDirection = direction;

        if (model != null)
        {
            SetMoveSpeed(model.moveSpeed);
        }
        else
        {
            SetMoveSpeed(0);
        }
    }

    public virtual void Walk()
    {
        if (currentMovementState == (int)MovementState.Walk)
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
        if (currentMovementState == (int)MovementState.Walk)
        {
            currentMovementState = (int)MovementState.Slash;
            animator.SetInteger("State", currentMovementState);
        }
    }

    public virtual void SlashDelayTiming()
    {
        if (currentMovementState == (int)MovementState.SlashDelay)
        {
            slashDelayTimer += Time.deltaTime;
            if (slashDelayTimer >= slashDelay)
            {
                currentMovementState = (int)MovementState.Slash;
                animator.SetInteger("State", currentMovementState);
            }
        }
    }

    public virtual void SlashEnd()
    {
        currentMovementState = (int)MovementState.SlashDelay;
        animator.SetInteger("State", currentMovementState);
        slashDelayTimer = 0;
    }

    public virtual void Flip(bool direction)
    {
            Vector3 theScale = transform.localScale;
            if (direction)
                theScale.x = 1;
            else
                theScale.x = -1;
            transform.localScale = theScale;
    }

    public virtual void TakeDamage()
    {
        EnemySpawnManager.Instance.EnemyKilled();
        Destroy(gameObject);
    }

    public virtual void SetMoveSpeed(float moveSpeed) {
        this.moveSpeed = moveSpeed;
    }

    public virtual void Pause()
    {
        pause = true;
        DisableDamageBox();
    }

    public virtual void UnPause()
    {
        pause = false;
        animator.SetInteger("State", currentMovementState);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWarrior : GoblinSwordman
{

    public int health;
    public float recoverMoveSpeed;
    public float knockBackDistance;
    public float knockBackDuration;
    protected float knockBackDurationTimer;
    public float knockBackDelay;
    protected float knockBackDelayTimer;
    public float recoverDuration;
    protected float recoverDurationTimer;

    public enum MovementState
    {
        Walk,
        Slash,
        SlashDelay,
        KnockedBack,
        Recover,
    }

    void Update()
    {
        Walk();
        SlashDelayTiming();
        KnockBackTiming();
        RecoverTiming();
    }

    public override void Setup(bool direction, ModelLevel model = null)
    {
        Flip(direction);
        spawnDirection = direction;
    }

    public override void Walk()
    {
        if (currentMovementState == (int)MovementState.Walk && health > 1)
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
        else if (currentMovementState == (int)MovementState.Walk && health <= 1)
        {
            if (spawnDirection == true)
            {
                gameObject.transform.position += new Vector3(recoverMoveSpeed, 0) * Time.deltaTime;
            }
            else
            {
                gameObject.transform.position -= new Vector3(recoverMoveSpeed, 0) * Time.deltaTime;
            }
        }
    }
    public virtual void KnockBackTiming()
    {
        if (currentMovementState == (int)MovementState.KnockedBack)
        {
            knockBackDurationTimer += Time.deltaTime;
            knockBackDelayTimer += Time.deltaTime;

            if (knockBackDurationTimer >= knockBackDuration)
            {

            }
            else
            {
                if (spawnDirection == true)
                {
                    gameObject.transform.position -= new Vector3(knockBackDistance, 0) * Time.deltaTime;
                }
                else
                {
                    gameObject.transform.position += new Vector3(knockBackDistance, 0) * Time.deltaTime;
                }
            }

            if (knockBackDelayTimer >= knockBackDelay)
            {
                recoverDurationTimer = 0;
                currentMovementState = (int)MovementState.Recover;
                animator.SetInteger("State", currentMovementState);
            }
        }
    }

    public virtual void RecoverTiming()
    {
        if (currentMovementState == (int)MovementState.Recover)
        {
            recoverDurationTimer += Time.deltaTime;

            if (recoverDurationTimer >= recoverDuration)
            {
                currentMovementState = (int)MovementState.Walk;
                animator.SetInteger("State", currentMovementState);
            }
        }
    }

    public override void TakeDamage()
    {
        health -= 1;
        if (health == 1)
        {
            knockBackDurationTimer = 0;
            knockBackDelayTimer = 0;
            currentMovementState = (int)MovementState.KnockedBack;
            animator.SetInteger("State", currentMovementState);
            KnockBackTiming();
        }
        else
            Destroy(gameObject);
    }
}

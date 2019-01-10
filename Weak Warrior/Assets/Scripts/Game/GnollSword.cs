using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnollSword : GoblinSwordman {

	public enum MovementState
    {
        Walk,
        Slash,
        SlashDelay,
        Stand,
    }

    public float standDelay;
    protected float standDelayTimer;

    public float standDuration;
    protected float standDurationTimer;

    void Start()
    {
        currentMovementState = (int)MovementState.Walk;
        animator.SetInteger("State", currentMovementState);
        standDelayTimer = 0;
    }

    void Update()
    {
        Walk();
        SlashDelayTiming();
        StandTiming();
    }

    public virtual void StandTiming()
    {
        if (currentMovementState == (int)MovementState.Walk)
        {
            standDelayTimer += Time.deltaTime;
            if (standDelayTimer >= standDelay)
            {
                standDurationTimer = 0;
                currentMovementState = (int)MovementState.Stand;
                animator.SetInteger("State", currentMovementState);
            }
        }
        else
            if (currentMovementState == (int)MovementState.Stand)
            {
                standDurationTimer += Time.deltaTime;
                if (standDurationTimer >= standDuration)
                {
                    standDelayTimer = 0;
                    currentMovementState = (int)MovementState.Walk;
                    animator.SetInteger("State", currentMovementState);
                }
            }
    }

    public override void Slash()
    {
        if (currentMovementState == (int)MovementState.Walk || currentMovementState == (int)MovementState.Stand)
        {
            currentMovementState = (int)MovementState.Slash;
            animator.SetInteger("State", currentMovementState);
        }
    }
}

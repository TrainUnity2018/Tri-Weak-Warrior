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
        Die,
    }

    void Update()
    {
        if (!pause)
        {
            Walk();
            SlashDelayTiming();
            KnockBackTiming();
            RecoverTiming();
        }
        OnDead();
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
        if (PlayerStateControl.Instance.currentMovementState == (int)PlayerStateControl.MovementState.Slash)
            health -= 1;
        else if (PlayerStateControl.Instance.currentMovementState == (int)PlayerStateControl.MovementState.Dash)
            health -= 2;
        if (health == 1)
        {
            knockBackDurationTimer = 0;
            knockBackDelayTimer = 0;
            currentMovementState = (int)MovementState.KnockedBack;
            animator.SetInteger("State", currentMovementState);
            KnockBackTiming();
        }
        else
        {
            currentMovementState = (int)MovementState.Die;
            pause = true;
            body.GetComponent<SpriteRenderer>().enabled = true;
            head.GetComponent<SpriteRenderer>().enabled = true;
            headSplashSpeedHorizontal = headSplashStartSpeedHorizontal;
            headSplashSpeedVertical = headSplashStartSpeedVertical;
            headSplashSpinningSpeed = headSplashStartSpinningSpeed;
            bodySplashSpeedHorizontal = bodySplashStartSpeedHorizontal;
            bodySplashSpeedVertical = bodySplashStartSpeedVertical;
        }
    }

    public override void OnDead()
    {
        if (body.GetComponent<GoblinSwordman_BodyPart>().boundTouched && head.GetComponent<GoblinSwordman_BodyPart>().boundTouched)
        {
            Destroy(gameObject);
            EnemySpawnManager.Instance.EnemyKilled();
        }

        if (currentMovementState == (int)MovementState.Die)
        {
            DisableDamageBox();
            DisableHitBox();
            if (spawnDirection)
            {
                if (headSplashSpeedHorizontal > 0)
                {
                    head.gameObject.transform.position += new Vector3(headSplashSpeedHorizontal, 0) * Time.deltaTime;
                    headSplashSpeedHorizontal -= headSplashHorizontalDecelerate * Time.deltaTime;
                }
                head.gameObject.transform.position += new Vector3(0, headSplashSpeedVertical) * Time.deltaTime;
                headSplashSpeedVertical -= headSplashVerticalDecelerate * Time.deltaTime;
                if (headSplashSpinningSpeed > 0)
                {
                    float degreesPerSec = headSplashSpinningSpeed;
                    float rotAmount = degreesPerSec * Time.deltaTime;
                    float curRot = head.gameObject.transform.localRotation.eulerAngles.z;
                    head.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curRot + rotAmount));
                    headSplashSpinningSpeed -= headSplashSpinningDecelerate * Time.deltaTime;
                }

                if (bodySplashSpeedHorizontal > 0)
                {
                    body.gameObject.transform.position -= new Vector3(bodySplashSpeedHorizontal, 0) * Time.deltaTime;
                    bodySplashSpeedHorizontal -= bodySplashHorizontalDecelerate * Time.deltaTime;
                }
                body.gameObject.transform.position += new Vector3(0, bodySplashSpeedVertical) * Time.deltaTime;
                bodySplashSpeedVertical -= bodySplashVerticalDecelerate * Time.deltaTime;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                if (headSplashSpeedHorizontal > 0)
                {
                    head.gameObject.transform.position -= new Vector3(headSplashSpeedHorizontal, 0) * Time.deltaTime;
                    headSplashSpeedHorizontal -= headSplashHorizontalDecelerate * Time.deltaTime;
                }
                head.gameObject.transform.position += new Vector3(0, headSplashSpeedVertical) * Time.deltaTime;
                headSplashSpeedVertical -= headSplashVerticalDecelerate * Time.deltaTime;
                if (headSplashSpinningSpeed > 0)
                {
                    float degreesPerSec = headSplashSpinningSpeed;
                    float rotAmount = degreesPerSec * Time.deltaTime;
                    float curRot = head.gameObject.transform.localRotation.eulerAngles.z;
                    head.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curRot + rotAmount));
                    headSplashSpinningSpeed -= headSplashSpinningDecelerate * Time.deltaTime;
                }

                if (bodySplashSpeedHorizontal > 0)
                {
                    body.gameObject.transform.position += new Vector3(bodySplashSpeedHorizontal, 0) * Time.deltaTime;
                    bodySplashSpeedHorizontal -= bodySplashHorizontalDecelerate * Time.deltaTime;
                }
                body.gameObject.transform.position += new Vector3(0, bodySplashSpeedVertical) * Time.deltaTime;
                bodySplashSpeedVertical -= bodySplashVerticalDecelerate * Time.deltaTime;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
}

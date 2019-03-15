using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnollSword : GoblinSwordman
{

    public enum MovementState
    {
        Walk,
        Slash,
        SlashDelay,
        Stand,
        Die,
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

        //standDelay = Random.Range(standDelay, standDelay + 0.6f);
    }

    void Update()
    {
        if (!pause)
        {
            Walk();
            SlashDelayTiming();
            StandTiming();
        }
        OnDead();
    }

    public override void Setup(bool direction, ModelLevel model = null)
    {
        Flip(direction);
        spawnDirection = direction;
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

    public override void TakeDamage()
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

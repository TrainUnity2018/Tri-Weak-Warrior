using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpSword : GoblinSwordman
{

    public float jumpDecelerate;
    public float startJumpSpeed;
    protected float jumpSpeed;
    public float fallAccelerate;
    public float startFallSpeed;
    protected float fallSpeed;
    public bool isJump;

    void Start()
    {
        currentMovementState = (int)MovementState.Walk;
        //animator.SetInteger("State", currentMovementState);

        isJump = true;
        jumpSpeed = startJumpSpeed;
        fallSpeed = startFallSpeed;
    }

    void Update()
    {
        if (!pause)
        {
            Walk();
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

            if (isJump)
            {
                if (jumpSpeed <= 0)
                {
                    isJump = false;
                    fallSpeed = startFallSpeed;
                }
                else
                {
                    jumpSpeed -= jumpDecelerate * Time.deltaTime;
                    gameObject.transform.position += new Vector3(0, jumpSpeed) * Time.deltaTime;
                }
            }
            else
            {
                fallSpeed += fallAccelerate * Time.deltaTime;
                gameObject.transform.position -= new Vector3(0, fallSpeed) * Time.deltaTime;
            }
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

    public override void OnCollide(Collider2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isJump = true;
            jumpSpeed = startJumpSpeed;
        }
    }
}

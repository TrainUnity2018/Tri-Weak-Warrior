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

    public float superJumpDecelerate;
    public float superStartJumpSpeed;
    public float superFallAccelerate;
    public float superStartFallSpeed;

    public bool isJump;

    void Start()
    {
        EnableHitBox();
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
        if (!Popup.Instance.pauseDialog.activeSelf && !Popup.Instance.deadDiaglog.activeSelf)
        {
            OnDead();
        }
    }

    public override void Setup(bool direction, ModelLevel model = null)
    {
        Flip(direction);
        spawnDirection = direction;

        if (model != null)
        {
            isSuper = model.isSupper;
        }
        else
        {

        }
    }

    public override void Walk()
    {
        if (currentMovementState == (int)MovementState.Walk)
        {
            if (isSuper)
            {
                if (spawnDirection == true)
                {
                    gameObject.transform.position += new Vector3(superMoveSpeed, 0) * Time.deltaTime;
                }
                else
                {
                    gameObject.transform.position -= new Vector3(superMoveSpeed, 0) * Time.deltaTime;
                }

                if (isJump)
                {
                    if (jumpSpeed <= 0)
                    {
                        isJump = false;
                        fallSpeed = superStartFallSpeed;
                    }
                    else
                    {
                        jumpSpeed -= superJumpDecelerate * Time.deltaTime;
                        gameObject.transform.position += new Vector3(0, jumpSpeed) * Time.deltaTime;
                    }
                }
                else
                {
                    fallSpeed += superFallAccelerate * Time.deltaTime;
                    gameObject.transform.position -= new Vector3(0, fallSpeed) * Time.deltaTime;
                }
            }
            else
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
    }

    public override void TakeDamage(int playersMovementState)
    {
        currentMovementState = (int)MovementState.Die;
        this.Pause();
        body.GetComponent<SpriteRenderer>().enabled = true;
        head.GetComponent<SpriteRenderer>().enabled = true;
        dieEffect.GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(DieEffect());

        headSplashSpinningSpeed = headSplashStartSpinningSpeed;
        headSplashVelocity = headSplashStartVelocity;
        bodySplashVelocity = bodySplashStartVelocity;
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
                if (headSplashVelocity.x > 0)
                {
                    head.gameObject.transform.position += new Vector3(headSplashVelocity.x, 0) * Time.deltaTime;
                }
                head.gameObject.transform.position += new Vector3(0, headSplashVelocity.y) * Time.deltaTime;
                headSplashVelocity = new Vector2(headSplashVelocity.x - headSplashDecelerate.x * Time.deltaTime, headSplashVelocity.y - headSplashDecelerate.y * Time.deltaTime);
                headSplashDecelerate = new Vector2(headSplashDecelerate.x - Time.deltaTime * 10, headSplashDecelerate.y);

                if (headSplashSpinningSpeed > 0)
                {
                    float degreesPerSec = headSplashSpinningSpeed;
                    float rotAmount = degreesPerSec * Time.deltaTime;
                    float curRot = head.gameObject.transform.localRotation.eulerAngles.z;
                    head.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curRot + rotAmount));
                    headSplashSpinningSpeed -= headSplashSpinningDecelerate * Time.deltaTime;
                }

                if (bodySplashVelocity.x > 0)
                {
                    body.gameObject.transform.position -= new Vector3(bodySplashVelocity.x, 0) * Time.deltaTime;
                }
                body.gameObject.transform.position += new Vector3(0, bodySplashVelocity.y) * Time.deltaTime;
                bodySplashVelocity = new Vector2(bodySplashVelocity.x - bodySplashDecelerate.x * Time.deltaTime, bodySplashVelocity.y - bodySplashDecelerate.y * Time.deltaTime);
                bodySplashDecelerate = new Vector2(bodySplashDecelerate.x - Time.deltaTime * 10, bodySplashDecelerate.y);

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                if (headSplashVelocity.x > 0)
                {
                    head.gameObject.transform.position -= new Vector3(headSplashVelocity.x, 0) * Time.deltaTime;
                }
                head.gameObject.transform.position += new Vector3(0, headSplashVelocity.y) * Time.deltaTime;
                headSplashVelocity = new Vector2(headSplashVelocity.x - headSplashDecelerate.x * Time.deltaTime, headSplashVelocity.y - headSplashDecelerate.y * Time.deltaTime);
                headSplashDecelerate = new Vector2(headSplashDecelerate.x - Time.deltaTime * 10, headSplashDecelerate.y);

                if (headSplashSpinningSpeed > 0)
                {
                    float degreesPerSec = headSplashSpinningSpeed;
                    float rotAmount = degreesPerSec * Time.deltaTime;
                    float curRot = head.gameObject.transform.localRotation.eulerAngles.z;
                    head.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curRot + rotAmount));
                    headSplashSpinningSpeed -= headSplashSpinningDecelerate * Time.deltaTime;
                }

                if (bodySplashVelocity.x > 0)
                {
                    body.gameObject.transform.position += new Vector3(bodySplashVelocity.x, 0) * Time.deltaTime;
                }
                body.gameObject.transform.position += new Vector3(0, bodySplashVelocity.y) * Time.deltaTime;
                bodySplashVelocity = new Vector2(bodySplashVelocity.x - bodySplashDecelerate.x * Time.deltaTime, bodySplashVelocity.y - bodySplashDecelerate.y * Time.deltaTime);
                bodySplashDecelerate = new Vector2(bodySplashDecelerate.x - Time.deltaTime * 10, bodySplashDecelerate.y);

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    public override void OnCollide(Collider2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isJump = true;
            if (isSuper)
            {
                jumpSpeed = superStartJumpSpeed;
            }
            else
            {
                jumpSpeed = startJumpSpeed;
            }            
        }
    }

    IEnumerator DieEffect()
    {
        while (true)
        {
            yield return new WaitForSeconds(.2f);
            dieEffect.GetComponent<SpriteRenderer>().enabled = false;
            StopCoroutine(DieEffect());
        }
    }
}

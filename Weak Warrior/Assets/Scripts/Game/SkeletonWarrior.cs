using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWarrior : GoblinSwordman
{    

    public AudioClip recoverSound;
    protected bool recoverSoundPlayed;

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
        if (!Popup.Instance.pauseDialog.activeSelf && !Popup.Instance.deadDiaglog.activeSelf)
        {
            OnDead();
        }  
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
                if (recoverSoundPlayed == false)
                {
                    audioSource.clip = recoverSound;
                    audioSource.Play(0);
                    recoverSoundPlayed = true;
                }
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

    public override void TakeDamage(int playersMovementState)
    {
        EnableHitBox();
        
        if (playersMovementState == (int)PlayerStateControl.MovementState.Slash) {
            health -= 1;
            audioSource.clip = hitSound;
            audioSource.Play(0);
            if (health <= 0)
                health = 0;
        }
            
        if (playersMovementState == (int)PlayerStateControl.MovementState.Dash) {
            health -= health;
            audioSource.clip = hitSound;
            audioSource.Play(0);
            if (health <= 0)
                health = 0;
        }

        if (health >= 1)
        {
            knockBackDurationTimer = 0;
            knockBackDelayTimer = 0;
            currentMovementState = (int)MovementState.KnockedBack;
            animator.SetInteger("State", currentMovementState);
            recoverSoundPlayed = false;
        }
        if (health == 0)
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

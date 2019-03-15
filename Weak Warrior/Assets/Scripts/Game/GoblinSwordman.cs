using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSwordman : MonoBehaviour
{

    public float moveSpeed;
    public int damage;
    public bool spawnDirection;

    public float slashDelay;
    protected float slashDelayTimer;

    public Animator animator;

    public GoblinSwordman_DamageBox damageBox;
    public BoxCollider2D hitBox;

    public GameObject head;
    public GameObject body;
    public float bodySplashStartSpeedVertical;
    public float bodySplashStartSpeedHorizontal;
    protected float bodySplashSpeedVertical;
    protected float bodySplashSpeedHorizontal;
    public float bodySplashVerticalDecelerate;
    public float bodySplashHorizontalDecelerate;

    public float headSplashStartSpeedVertical;
    public float headSplashStartSpeedHorizontal;
    protected float headSplashSpeedVertical;
    protected float headSplashSpeedHorizontal;
    public float headSplashVerticalDecelerate;
    public float headSplashHorizontalDecelerate;
    public float headSplashStartSpinningSpeed;
    protected float headSplashSpinningSpeed;
    public float headSplashSpinningDecelerate;

    public bool pause;

    public enum MovementState
    {
        Walk,
        Slash,
        SlashDelay,
        Die,
    }

    public int currentMovementState;

    // Use this for initialization
    void Start()
    {
        currentMovementState = (int)MovementState.Walk;
        animator.SetInteger("State", currentMovementState);
        pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            Walk();
            SlashDelayTiming();
        }
        OnDead();
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
        if (currentMovementState != (int)MovementState.Die)
        {
            currentMovementState = (int)MovementState.SlashDelay;
            animator.SetInteger("State", currentMovementState);
            slashDelayTimer = 0;
        }
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

    public virtual void OnDead()
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

    public virtual void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public virtual void Pause()
    {
        pause = true;
        DisableDamageBox();
        DisableHitBox();
    }

    public virtual void UnPause()
    {
        pause = false;
        animator.SetInteger("State", currentMovementState);
    }

    public virtual void EnableHitBox()
    {
        if (currentMovementState != (int)MovementState.Die)
        {
            hitBox.enabled = true;
        }

    }

    public virtual void DisableHitBox()
    {
        hitBox.enabled = false;
    }

    public virtual void EnableDamageBox()
    {
        if (currentMovementState != (int)MovementState.Die)
        {
            damageBox.EnableDamageBox();
        } 
    }

    public virtual void DisableDamageBox()
    {
        damageBox.DisableDamageBox();      
    }

    public virtual void OnCollide(Collider2D col)
    {

    }
}

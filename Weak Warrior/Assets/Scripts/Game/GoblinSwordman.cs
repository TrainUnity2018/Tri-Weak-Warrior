using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoblinSwordman : MonoBehaviour
{

    public float moveSpeed;
    public float superMoveSpeed;
    public int damage;
    public bool spawnDirection;

    public float slashDelay;
    protected float slashDelayTimer;

    public Animator animator;

    public GoblinSwordman_DamageBox damageBox;
    public BoxCollider2D hitBox;
    public bool isUlted;

    public GameObject head;
    public GameObject body;
    public GameObject dieEffect;

    public Vector2 bodySplashStartVelocity;
    protected Vector2 bodySplashVelocity;
    public Vector2 bodySplashDecelerate;

    public float headSplashStartSpinningSpeed;
    protected float headSplashSpinningSpeed;
    public float headSplashSpinningDecelerate;

    public Vector2 headSplashStartVelocity;
    protected Vector2 headSplashVelocity;
    public Vector2 headSplashDecelerate;

    public bool pause;
    public bool isSuper;

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
        isUlted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            Walk();
            SlashDelayTiming();
        }
        if (!Popup.Instance.pauseDialog.activeSelf && !Popup.Instance.deadDiaglog.activeSelf)
        {
            OnDead();
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
            isSuper = model.isSupper;
        }
        else
        {

        }
    }

    public virtual void Walk()
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
            theScale.x = transform.localScale.x;
        else
            theScale.x = -transform.localScale.x;
        transform.localScale = theScale;
    }

    public virtual void TakeDamage(int playersMovementSate)
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

    public virtual void Pause()
    {
        if (currentMovementState != (int)MovementState.Die)
        {
            pause = true;
            DisableDamageBox();
            DisableHitBox();
        }
    }

    public virtual void UnPause()
    {
        pause = false;
        EnableHitBox();
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
            if (damageBox != null)
                damageBox.EnableDamageBox();
        }
    }

    public virtual void DisableDamageBox()
    {
        if (damageBox != null)
            damageBox.DisableDamageBox();
    }

    public virtual void AfterDash()
    {
        isUlted = false;
    }

    public virtual void OnCollide(Collider2D col)
    {

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

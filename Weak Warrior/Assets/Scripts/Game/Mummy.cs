using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mummy : GoblinSwordman
{

    public int health;
    protected int currentHealth;
    protected int initHealth;
    public GameObject healthBar;
    public GameObject healthBarFrame;
    public float knockBackDistance;
    public float knockBackDuration;
    protected float knockBackDurationTimer;
    public float knockBackDelay;
    protected float knockBackDelayTimer;
    public enum MovementState
    {
        Walk,
        Slash,
        SlashDelay,
        KnockedBack,
        Die,
    }

    // Use this for initialization
    void Start()
    {
        EnableHitBox();
        currentMovementState = (int)MovementState.Walk;
        animator.SetInteger("State", currentMovementState);
        pause = false;
        initHealth = health;
        currentHealth = initHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            Walk();
            SlashDelayTiming();
            KnockBackTiming();
        }
        if (!Popup.Instance.pauseDialog.activeSelf && !Popup.Instance.deadDiaglog.activeSelf)
        {
            OnDead();
        }
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
                currentMovementState = (int)MovementState.Walk;
                animator.SetInteger("State", currentMovementState);
            }
        }
    }

    public override void TakeDamage(int playersMovementState)
    {
        EnableHitBox();
        StartCoroutine(HitEffect());

        if (playersMovementState == (int)PlayerStateControl.MovementState.Slash)
        {
            health -= 1;
            if (health <= 0)
                health = 0;
            currentHealth = health;
        }
        if (playersMovementState == (int)PlayerStateControl.MovementState.Dash)
        {
            health -= health;
            if (health <= 0)
                health = 0;
            currentHealth = health;
        }

        HealthBarScale();

        if (health > 0)
        {
            knockBackDurationTimer = 0;
            knockBackDelayTimer = 0;
            currentMovementState = (int)MovementState.KnockedBack;
            animator.SetInteger("State", currentMovementState);
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
            healthBarFrame.SetActive(false);
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

    private void HealthBarScale()
    {
        float scaleRatio = (float)((float)currentHealth / (float)initHealth);
        Vector3 healthBarScale = healthBar.transform.localScale;
        healthBarScale.x = 0.1f * scaleRatio;
        healthBar.transform.localScale = healthBarScale;
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

    IEnumerator HitEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            Color color = new Color((float)(255 / 255), (float)(61 / 255), (float)(61 / 255), (float)(255 / 255));
            this.gameObject.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(.05f);
            color = new Color((float)(255 / 255), (float)(255 / 255), (float)(255 / 255), (float)(255 / 255));
            this.gameObject.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(.05f);
        }
        StopCoroutine(HitEffect());
    }
}

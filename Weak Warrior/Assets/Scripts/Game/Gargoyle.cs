using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : GoblinSwordman
{

    public Vector3 attackPositionLeft;
    public Vector3 attackPositionRight;
    public Vector3 playerPosition;

    public bool walkout;
    public bool leftoright;

    protected float count;
	public float flyingDelay;
	protected float flyingDelayTimer;

	public int health;

	public Gargoyle_DamageBox damageBoxGargoyle;

    void Start()
    {
        EnableHitBox();
        currentMovementState = (int)MovementState.Walk;
        //animator.SetInteger("State", currentMovementState);
        walkout = true;
        leftoright = spawnDirection;
		count = 0;
		flyingDelayTimer = flyingDelay;
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


        walkout = true;
        if (spawnDirection)
        {

            leftoright = true;
        }
        else
        {
            leftoright = false;
        }

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
            if (walkout)
            {
                if (spawnDirection)
                {
                    if (transform.position.x < attackPositionLeft.x)
                    {
                        transform.position += new Vector3(moveSpeed, 0) * Time.deltaTime;
                    }
                    else
                    {
                        walkout = false;
                    }
                }
                else
                {
                    if (transform.position.x > attackPositionRight.x)
                    {
                        transform.position -= new Vector3(moveSpeed, 0) * Time.deltaTime;
                    }
                    else
                    {
                        walkout = false;
                    }
                }
            }
            else
            {
                flyingDelayTimer += Time.deltaTime;
				if (flyingDelayTimer >= flyingDelay) {
                    if (leftoright)
                    {
                        if (count < 1.0f)
                        {
                            count += 1.0f * Time.deltaTime;

                            Vector3 m1 = Vector3.Lerp(attackPositionLeft, playerPosition, count);
                            Vector3 m2 = Vector3.Lerp(playerPosition, attackPositionRight, count);
                            this.gameObject.transform.position = Vector3.Lerp(m1, m2, count);
                        }
                        else
                        {
                            Flip(false);
                            leftoright = false;
                            count = 0;
							flyingDelayTimer = 0;
							EnableDamageBox();
                        }
                    }
                    else
                    {
                        if (count < 1.0f)
                        {
                            count += 1.0f * Time.deltaTime;

                            Vector3 m1 = Vector3.Lerp(attackPositionRight, playerPosition, count);
                            Vector3 m2 = Vector3.Lerp(playerPosition, attackPositionLeft, count);
                            this.gameObject.transform.position = Vector3.Lerp(m1, m2, count);
                        }
                        else
                        {
                            Flip(false);
                            leftoright = true;
                            count = 0;
                            flyingDelayTimer = 0;
                            EnableDamageBox();
                        }
                    }
				}			
            }
        }
    }

    public override void TakeDamage(int playersMovementState)
    {
        EnableHitBox();

        if (playersMovementState == (int)PlayerStateControl.MovementState.Slash)
        {
            health -= 1;
            audioSource.clip = hitSound;
            audioSource.Play(0);
            if (health <= 0)
                health = 0;
        }
        if (playersMovementState == (int)PlayerStateControl.MovementState.Dash)
        {
            health -= health;
            audioSource.clip = hitSound;
            audioSource.Play(0);
            if (health <= 0)
                health = 0;
        }
		
		if (health == 0) {
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

	public override void DisableDamageBox() {
		damageBoxGargoyle.DisableDamageBox();
	}

	public override void EnableDamageBox() {
		damageBoxGargoyle.EnableDamageBox();
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

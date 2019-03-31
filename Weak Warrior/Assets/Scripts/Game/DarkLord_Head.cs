using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkLord_Head : DarkTree_Arm
{

    public Vector3 leftFloatingPosition;
    public Vector3 rightFloatingPosition;
    public Vector3 middleFloatingPosition;
    public float floatingSpeedAccelerate;

    protected float count;
    protected Quaternion rotateAngle;
    Vector3 enemyTarget = new Vector3(0, -1.3f, 0);

    public bool floatingleftToRight;
    public float floatingHorizontalDelay;
    protected float floatingHorizontalDelayTimer;

    // Use this for initialization
    void Start()
    {
        DisableDamageBox();
		DisableHitBox();
		currentMovementState = -1;
        //currentMovementState = (int)MovementState.PrepareAttack;
        //currentMovementState = (int)MovementState.Floating;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!body.pause && (PlayerStateControl.Instance.currentMovementState != (int)PlayerStateControl.MovementState.Dash))
        {
            Floating();
            FindRotateAngle();
            Attack();
            Retreat();
            KnockBack();
        }
        else if (body.pause && (PlayerStateControl.Instance.currentMovementState == (int)PlayerStateControl.MovementState.Dash))
        {
            KnockBack();
        }
        else if (!body.pause && (PlayerStateControl.Instance.currentMovementState == (int)PlayerStateControl.MovementState.Dash))
        {
            KnockBack();
        }
    }

    public override void Setup() {
        EnableDamageBox();
		EnableHitBox();
		nextFloatingPosition = attackStartPosition;
        currentMovementState = (int)MovementState.Floating;
        floatingDelayTimer = 0;
        floatingDelay = floatingDelayAverage + Random.Range(-floatingDelayAverage, floatingDelayRange);
	}
	
	public override void Floating()
    {
        if (currentMovementState == (int)MovementState.Floating)
        {
            EnableHitBox();
            floatingHorizontalDelayTimer += Time.deltaTime;
            if (floatingHorizontalDelayTimer >= floatingHorizontalDelay)
            {
                if (floatingleftToRight)
                {
                    if (count < 1.0f)
                    {
                        count += 1.0f * Time.deltaTime;
                        Vector3 m1 = Vector3.Lerp(leftFloatingPosition, middleFloatingPosition, count);
                        Vector3 m2 = Vector3.Lerp(middleFloatingPosition, rightFloatingPosition, count);
                        this.gameObject.transform.position = Vector3.Lerp(m1, m2, count);
                        this.gameObject.transform.rotation = rotateAngle;
                    }
                    else
                    {
                        count = 0;
                        floatingleftToRight = false;
                        floatingHorizontalDelayTimer = 0;
                        attackStartPosition = new Vector3(this.transform.position.x, this.transform.position.y);
                        attackEndPosition = enemyTarget;
                    }
                }
                else
                {
                    if (count < 1.0f)
                    {
                        count += 1.0f * Time.deltaTime;
                        Vector3 m1 = Vector3.Lerp(rightFloatingPosition, middleFloatingPosition, count);
                        Vector3 m2 = Vector3.Lerp(middleFloatingPosition, leftFloatingPosition, count);
                        this.gameObject.transform.position = Vector3.Lerp(m1, m2, count);
                        this.gameObject.transform.rotation = rotateAngle;
                    }
                    else
                    {
                        count = 0;
                        floatingleftToRight = true;
                        floatingHorizontalDelayTimer = 0;
						attackStartPosition = new Vector3(this.transform.position.x, this.transform.position.y);
						attackEndPosition = enemyTarget;
                    }
                }
            }
            else
            {
                floatingDelayTimer += Time.deltaTime;
                if (floatingDelayTimer >= floatingDelay)
                {
                    Vector3 direction = this.transform.position - enemyTarget;
                    if ((Mathf.Abs(transform.localPosition.x - nextFloatingPosition.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - nextFloatingPosition.y) > 0.01f))
                    {
                        Vector3 moveVector = (nextFloatingPosition - transform.localPosition).normalized;
                        transform.localPosition += moveVector * floatingSpeed * Time.deltaTime;
                    }
                    else
                    {
                        floatingDelayTimer = 0;
                        floatingDelay = floatingDelayAverage + Random.Range(-floatingDelayAverage, floatingDelayRange);
                        nextFloatingPosition = new Vector3(Random.Range(this.transform.position.x - direction.x * floatingRange, this.transform.position.x + direction.x * floatingRange), Random.Range(this.transform.position.y - direction.y * floatingRange, this.transform.position.y + direction.y * floatingRange));
                    }
                }
            }
        }
    }

    public override void Attack()
    {
        if (currentMovementState == (int)MovementState.PrepareAttack)
        {
            if ((Mathf.Abs(transform.localPosition.x - attackStartPosition.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - attackStartPosition.y) > 0.01f))
            {
                Vector3 moveVector = (attackStartPosition - transform.localPosition).normalized;
                transform.localPosition += moveVector * floatingSpeed * Time.deltaTime;
            }
            else
            {
                currentMovementState = (int)MovementState.Attack;
                attackVibrateTimer = 0;
            }
        }

        if (currentMovementState == (int)MovementState.Attack)
        {
            attackVibrateTimer += Time.deltaTime;
            if (attackVibrateTimer > attackVibrateDuration)
            {
                EnableDamageBox();
                if ((Mathf.Abs(transform.localPosition.x - attackEndPosition.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - attackEndPosition.y) > 0.01f))
                {
                    Vector3 moveVector = (attackEndPosition - transform.localPosition).normalized;
                    transform.localPosition += moveVector * attackSpeed * Time.deltaTime;
                }
                else
                {
                    currentMovementState = (int)MovementState.Retreat;
                    attackDelayTimer = 0;
                }
            }
            else
            {
                nextFloatingPosition = new Vector3(Random.Range(attackStartPosition.x - 0.1f, attackStartPosition.x + 0.1f), Random.Range(attackStartPosition.y - 0.1f, attackStartPosition.y + 0.1f));
                if ((Mathf.Abs(transform.localPosition.x - nextFloatingPosition.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - nextFloatingPosition.y) > 0.01f))
                {
                    Vector3 moveVector = (nextFloatingPosition - transform.localPosition).normalized;
                    transform.localPosition += moveVector * 5 * Time.deltaTime;
                }
                else
                {
                    Vector3 direction = this.transform.position - enemyTarget;
                    nextFloatingPosition = new Vector3(Random.Range(this.transform.position.x - direction.x * floatingRange, this.transform.position.x + direction.x * floatingRange), Random.Range(this.transform.position.y - direction.y * floatingRange, this.transform.position.y + direction.y * floatingRange));
                }
            }
        }
    }

    public override void Retreat()
    {
        if (currentMovementState == (int)MovementState.Retreat)
        {
            DisableDamageBox();
            attackDelayTimer += Time.deltaTime;
            if (attackDelayTimer >= attackDelay)
            {
                if ((Mathf.Abs(transform.localPosition.x - attackStartPosition.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - attackStartPosition.y) > 0.01f))
                {
                    Vector3 moveVector = (attackStartPosition - transform.localPosition).normalized;
                    transform.localPosition += moveVector * retreatSpeed * Time.deltaTime;
                }
                else
                {
                    currentMovementState = (int)MovementState.Floating;
                }
            }

        }
    }

    public override void KnockBack()
    {
        if (currentMovementState == (int)MovementState.KnockBack)
        {
            DisableDamageBox();
            DisableHitBox();
            if ((Mathf.Abs(transform.localPosition.x - attackStartPosition.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - attackStartPosition.y) > 0.01f))
            {
                Vector3 moveVector = (attackStartPosition - transform.localPosition).normalized;
                transform.localPosition += moveVector * knockBackSpeed * Time.deltaTime;
            }
            else
            {
                currentMovementState = (int)MovementState.Floating;
            }
        }
    }

    public virtual void FindRotateAngle()
    {
        Vector3 direction = this.transform.position - enemyTarget;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        rotateAngle = Quaternion.Slerp(transform.rotation, (Quaternion.AngleAxis(angle, Vector3.forward)), 10f * Time.deltaTime);
    }
}

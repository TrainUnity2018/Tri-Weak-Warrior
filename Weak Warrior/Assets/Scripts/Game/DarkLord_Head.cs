using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkLord_Head : DarkTree_Arm
{

    public bool firstHorizontalFloating;
    public Vector3 firstRightHorizontalFloatingPosition;
    public Vector3 firstMiddleHorizontalFloatingPosition;

    public Vector3 attackStartPosition2;
    public bool currentAttackPosition;

    public float prepareAttackSpeed;

    public Vector3 leftHorizontalFloatingPosition;
    public Vector3 rightHorizontalFloatingPosition;
    public Vector3 middleHorizontalFloatingPosition;

    public bool fallBackFromFloating;

    public float count;
    protected Quaternion rotateAngle;
    protected Vector3 enemyTarget = new Vector3(0, -1.3f, 0);
    protected Vector3 firstPosition = new Vector3(0, 1.6f, 0);

    public bool floatingLeftToRight;
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

        FindRotateAngle();
    }

    public override void Setup()
    {
        EnableDamageBox();
        EnableHitBox();
        firstHorizontalFloating = true;
        nextFloatingPosition = attackStartPosition;
        currentMovementState = (int)MovementState.Floating;
        //currentMovementState = -1;
        count = 0;
        floatingDelayTimer = 0;
        floatingDelay = floatingDelayAverage + Random.Range(-floatingDelayAverage, floatingDelayRange);
        isUlted = false;
    }

    public override void Floating()
    {
        if (currentMovementState == (int)MovementState.Floating)
        {
            if (firstHorizontalFloating)
            {
                if (count < 1.0f)
                {
                    count += Time.deltaTime;
                    Vector3 m1 = Vector3.Lerp(firstRightHorizontalFloatingPosition, firstMiddleHorizontalFloatingPosition, count);
                    Vector3 m2 = Vector3.Lerp(firstMiddleHorizontalFloatingPosition, leftHorizontalFloatingPosition, count);
                    this.gameObject.transform.position = Vector3.Lerp(m1, m2, count);
                    this.gameObject.transform.rotation = rotateAngle;
                }
                else
                {
                    count = 0;
                    firstHorizontalFloating = false;
                    floatingLeftToRight = true;
                    currentAttackPosition = true;
                    nextFloatingPosition = attackStartPosition;
                    floatingHorizontalDelayTimer = 0;
                }
            }
            else
            {
                EnableHitBox();
                floatingHorizontalDelayTimer += Time.deltaTime;
                if (floatingHorizontalDelayTimer >= floatingHorizontalDelay)
                {
                    if (floatingLeftToRight)
                    {
                        if (fallBackFromFloating)
                        {
                            if ((Mathf.Abs(transform.localPosition.x - attackStartPosition.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - attackStartPosition.y) > 0.01f))
                            {
                                Vector3 moveVector = (attackStartPosition - transform.localPosition).normalized;
                                transform.localPosition += moveVector * floatingSpeed * Time.deltaTime;
                                this.gameObject.transform.rotation = rotateAngle;
                            }
                            else
                            {
                                transform.localPosition = new Vector3(attackStartPosition.x, attackStartPosition.y);
                                fallBackFromFloating = false;
                            }
                        }
                        else
                        {
                            if (count < 1.0f)
                            {
                                count += Time.deltaTime;
                                Vector3 m1 = Vector3.Lerp(leftHorizontalFloatingPosition, middleHorizontalFloatingPosition, count);
                                Vector3 m2 = Vector3.Lerp(middleHorizontalFloatingPosition, rightHorizontalFloatingPosition, count);
                                this.gameObject.transform.position = Vector3.Lerp(m1, m2, count);
                                this.gameObject.transform.rotation = rotateAngle;
                            }
                            else
                            {
                                count = 0;
                                floatingLeftToRight = false;
                                currentAttackPosition = false;
                                nextFloatingPosition = attackStartPosition2;
                                floatingHorizontalDelayTimer = 0;
                            }
                        }
                    }
                    else
                    {
                        if (fallBackFromFloating)
                        {
                            if ((Mathf.Abs(transform.localPosition.x - attackStartPosition2.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - attackStartPosition2.y) > 0.01f))
                            {
                                Vector3 moveVector = (attackStartPosition2 - transform.localPosition).normalized;
                                transform.localPosition += moveVector * floatingSpeed * Time.deltaTime;
                                this.gameObject.transform.rotation = rotateAngle;
                            }
                            else
                            {
                                transform.localPosition = new Vector3(attackStartPosition2.x, attackStartPosition2.y);
                                fallBackFromFloating = false;
                            }
                        }
                        else
                        {
                            if (count < 1.0f)
                            {
                                count += Time.deltaTime;
                                Vector3 m1 = Vector3.Lerp(rightHorizontalFloatingPosition, middleHorizontalFloatingPosition, count);
                                Vector3 m2 = Vector3.Lerp(middleHorizontalFloatingPosition, leftHorizontalFloatingPosition, count);
                                this.gameObject.transform.position = Vector3.Lerp(m1, m2, count);
                                this.gameObject.transform.rotation = rotateAngle;
                            }
                            else
                            {
                                count = 0;
                                floatingLeftToRight = true;
                                currentAttackPosition = true;
                                nextFloatingPosition = attackStartPosition;
                                floatingHorizontalDelayTimer = 0;
                            }
                        }
                    }
                }
                else
                {
                    fallBackFromFloating = true;
                    if (!floatingLeftToRight)
                    {
                        floatingDelayTimer += Time.deltaTime;
                        if (floatingDelayTimer >= floatingDelay)
                        {
                            if ((Mathf.Abs(transform.localPosition.x - nextFloatingPosition.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - nextFloatingPosition.y) > 0.01f))
                            {
                                Vector3 moveVector = (nextFloatingPosition - transform.localPosition).normalized;
                                transform.localPosition += moveVector * floatingSpeed * Time.deltaTime;
                                this.gameObject.transform.rotation = rotateAngle;
                            }
                            else
                            {
                                floatingDelayTimer = 0;
                                floatingDelay = floatingDelayAverage + Random.Range(-floatingDelayAverage, floatingDelayRange);
                                nextFloatingPosition = new Vector3(Random.Range(attackStartPosition.x - floatingRange, attackStartPosition.x + floatingRange), Random.Range(attackStartPosition.y - floatingRange, attackStartPosition.y + floatingRange));
                            }
                        }
                    }
                    else
                    {
                        floatingDelayTimer += Time.deltaTime;
                        if (floatingDelayTimer >= floatingDelay)
                        {
                            if ((Mathf.Abs(transform.localPosition.x - nextFloatingPosition.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - nextFloatingPosition.y) > 0.01f))
                            {
                                Vector3 moveVector = (nextFloatingPosition - transform.localPosition).normalized;
                                transform.localPosition += moveVector * floatingSpeed * Time.deltaTime;
                                this.gameObject.transform.rotation = rotateAngle;
                            }
                            else
                            {
                                floatingDelayTimer = 0;
                                floatingDelay = floatingDelayAverage + Random.Range(-floatingDelayAverage, floatingDelayRange);
                                nextFloatingPosition = new Vector3(Random.Range(attackStartPosition2.x - floatingRange, attackStartPosition2.x + floatingRange), Random.Range(attackStartPosition2.y - floatingRange, attackStartPosition2.y + floatingRange));
                            }
                        }
                    }
                }
            }
        }
    }

    public override void Attack()
    {
        if (currentMovementState == (int)MovementState.PrepareAttack)
        {
            if (currentAttackPosition)
            {
                if ((Mathf.Abs(transform.localPosition.x - attackStartPosition.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - attackStartPosition.y) > 0.01f))
                {
                    Vector3 moveVector = (attackStartPosition - transform.localPosition).normalized;
                    transform.localPosition += moveVector * prepareAttackSpeed * Time.deltaTime;
                    this.gameObject.transform.rotation = rotateAngle;
                }
                else
                {
                    transform.localPosition = new Vector3(attackStartPosition.x, attackStartPosition.y);
                    currentMovementState = (int)MovementState.Attack;
                    attackVibrateTimer = 0;
                }
            }
            else
            {
                if ((Mathf.Abs(transform.localPosition.x - attackStartPosition2.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - attackStartPosition2.y) > 0.01f))
                {
                    Vector3 moveVector = (attackStartPosition2 - transform.localPosition).normalized;
                    transform.localPosition += moveVector * prepareAttackSpeed * Time.deltaTime;
                    this.gameObject.transform.rotation = rotateAngle;
                }
                else
                {
                    transform.localPosition = new Vector3(attackStartPosition2.x, attackStartPosition2.y);
                    currentMovementState = (int)MovementState.Attack;
                    attackVibrateTimer = 0;
                }
            }
        }

        if (currentMovementState == (int)MovementState.Attack)
        {
            if (currentAttackPosition)
            {
                attackVibrateTimer += Time.deltaTime;
                if (attackVibrateTimer > attackVibrateDuration)
                {
                    EnableDamageBox();
                    if ((Mathf.Abs(transform.localPosition.x - attackEndPosition.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - attackEndPosition.y) > 0.01f))
                    {
                        Vector3 moveVector = (attackEndPosition - transform.localPosition).normalized;
                        transform.localPosition += moveVector * attackSpeed * Time.deltaTime;
                        this.gameObject.transform.rotation = rotateAngle;
                    }
                    else
                    {
                        transform.localPosition = new Vector3(attackEndPosition.x, attackEndPosition.y);
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
                        this.gameObject.transform.rotation = rotateAngle;
                    }
                    else
                    {
                        transform.localPosition = new Vector3(attackStartPosition.x, attackStartPosition.y);
                        nextFloatingPosition = new Vector3(Random.Range(attackStartPosition.x - 0.1f, attackStartPosition.x + 0.1f), Random.Range(attackStartPosition.y - 0.1f, attackStartPosition.y + 0.1f));
                    }
                }
            }
            else
            {
                attackVibrateTimer += Time.deltaTime;
                if (attackVibrateTimer > attackVibrateDuration)
                {
                    EnableDamageBox();
                    if ((Mathf.Abs(transform.localPosition.x - attackEndPosition.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - attackEndPosition.y) > 0.01f))
                    {
                        Vector3 moveVector = (attackEndPosition - transform.localPosition).normalized;
                        transform.localPosition += moveVector * attackSpeed * Time.deltaTime;
                        this.gameObject.transform.rotation = rotateAngle;
                    }
                    else
                    {
                        transform.localPosition = new Vector3(attackEndPosition.x, attackEndPosition.y);
                        currentMovementState = (int)MovementState.Retreat;
                        attackDelayTimer = 0;
                    }
                }
                else
                {
                    nextFloatingPosition = new Vector3(Random.Range(attackStartPosition2.x - 0.1f, attackStartPosition2.x + 0.1f), Random.Range(attackStartPosition2.y - 0.1f, attackStartPosition2.y + 0.1f));
                    if ((Mathf.Abs(transform.localPosition.x - nextFloatingPosition.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - nextFloatingPosition.y) > 0.01f))
                    {
                        Vector3 moveVector = (nextFloatingPosition - transform.localPosition).normalized;
                        transform.localPosition += moveVector * 5 * Time.deltaTime;
                        this.gameObject.transform.rotation = rotateAngle;
                    }
                    else
                    {
                        transform.localPosition = new Vector3(attackStartPosition2.x, attackStartPosition2.y);
                        nextFloatingPosition = new Vector3(Random.Range(attackStartPosition2.x - 0.1f, attackStartPosition2.x + 0.1f), Random.Range(attackStartPosition2.y - 0.1f, attackStartPosition2.y + 0.1f));
                    }
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
                if (currentAttackPosition)
                {
                    if ((Mathf.Abs(transform.localPosition.x - attackStartPosition.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - attackStartPosition.y) > 0.01f))
                    {
                        Vector3 moveVector = (attackStartPosition - transform.localPosition).normalized;
                        transform.localPosition += moveVector * retreatSpeed * Time.deltaTime;
                        this.gameObject.transform.rotation = rotateAngle;
                    }
                    else
                    {
                        currentMovementState = (int)MovementState.Floating;
                    }
                }
                else
                {
                    if ((Mathf.Abs(transform.localPosition.x - attackStartPosition2.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - attackStartPosition2.y) > 0.01f))
                    {
                        Vector3 moveVector = (attackStartPosition2 - transform.localPosition).normalized;
                        transform.localPosition += moveVector * retreatSpeed * Time.deltaTime;
                        this.gameObject.transform.rotation = rotateAngle;
                    }
                    else
                    {
                        currentMovementState = (int)MovementState.Floating;
                    }
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
            if (firstHorizontalFloating)
            {
                if ((Mathf.Abs(transform.localPosition.x - firstPosition.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - firstPosition.y) > 0.01f))
                {
                    Vector3 moveVector = (firstPosition - transform.localPosition).normalized;
                    transform.localPosition += moveVector * knockBackSpeed * Time.deltaTime;
                    this.gameObject.transform.rotation = rotateAngle;
                }
                else
                {
                    currentMovementState = (int)MovementState.Floating;
                    firstHorizontalFloating = true;
                }
            }
            else
            {
                if (currentAttackPosition)
                {
                    if ((Mathf.Abs(transform.localPosition.x - attackStartPosition.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - attackStartPosition.y) > 0.01f))
                    {
                        Vector3 moveVector = (attackStartPosition - transform.localPosition).normalized;
                        transform.localPosition += moveVector * knockBackSpeed * Time.deltaTime;
                        this.gameObject.transform.rotation = rotateAngle;
                    }
                    else
                    {
                        currentMovementState = (int)MovementState.Floating;
                    }
                }
                else
                {
                    if ((Mathf.Abs(transform.localPosition.x - attackStartPosition2.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - attackStartPosition2.y) > 0.01f))
                    {
                        Vector3 moveVector = (attackStartPosition2 - transform.localPosition).normalized;
                        transform.localPosition += moveVector * knockBackSpeed * Time.deltaTime;
                        this.gameObject.transform.rotation = rotateAngle;
                    }
                    else
                    {
                        currentMovementState = (int)MovementState.Floating;
                    }
                }
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

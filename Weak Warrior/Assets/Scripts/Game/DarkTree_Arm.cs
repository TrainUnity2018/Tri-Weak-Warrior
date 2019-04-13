using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkTree_Arm : MonoBehaviour
{

    public Vector3 attackStartPosition;
    public Vector3 attackEndPosition;
    public Vector3 nextFloatingPosition;
    public float attackSpeed;
    public float retreatSpeed;
    public float knockBackSpeed;
    public float floatingSpeed;
    public float floatingRange;
    public int damage;

    public float attackVibrateDuration;
    protected float attackVibrateTimer;
    public float attackDelay;
    protected float attackDelayTimer;

    public float floatingDelayAverage;
    public float floatingDelayRange;
    protected float floatingDelay;
    protected float floatingDelayTimer;

    public DarkTree body;
    public BoxCollider2D hitBox;
    public BoxCollider2D damageBox;
    public bool ultDamaged;
    public enum MovementState
    {
        Floating,
        PrepareAttack,
        Attack,
        Retreat,
        KnockBack,
    }

    public int currentMovementState;

    // Use this for initialization
    void Start()
    {
        currentMovementState = -1;
        //currentMovementState = (int)MovementState.PrepareAttack;
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
    }

    public virtual void Setup()
    {
        nextFloatingPosition = attackStartPosition;
        currentMovementState = (int)MovementState.Floating;
        floatingDelayTimer = 0;
        floatingDelay = floatingDelayAverage + Random.Range(-floatingDelayAverage, floatingDelayRange);
        ultDamaged = false;
    }

    public virtual void Floating()
    {
        if (currentMovementState == (int)MovementState.Floating)
        {
            EnableHitBox();
            floatingDelayTimer += Time.deltaTime;
            if (floatingDelayTimer >= floatingDelay)
            {
                if ((Mathf.Abs(transform.localPosition.x - nextFloatingPosition.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - nextFloatingPosition.y) > 0.01f))
                {
                    Vector3 moveVector = (nextFloatingPosition - transform.localPosition).normalized;
                    transform.localPosition += moveVector * floatingSpeed * Time.deltaTime;
                }
                else
                {
                    floatingDelayTimer = 0;
                    floatingDelay = floatingDelayAverage + Random.Range(-floatingDelayAverage, floatingDelayRange);
                    nextFloatingPosition = new Vector3(Random.Range(attackStartPosition.x - floatingRange, attackStartPosition.x + floatingRange), attackStartPosition.y);
                }
            }
        }
    }

    public virtual void PrepareAttack()
    {
        if (currentMovementState == (int)MovementState.Floating)
        {
            currentMovementState = (int)MovementState.PrepareAttack;
        }
    }

    public virtual void Attack()
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
                    nextFloatingPosition = new Vector3(Random.Range(attackStartPosition.x - 0.1f, attackStartPosition.x + 0.1f), Random.Range(attackStartPosition.y - 0.1f, attackStartPosition.y + 0.1f));
                }
            }
        }

    }

    public virtual void Retreat()
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

    public virtual void KnockBack()
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
                ultDamaged = true;
            }
        }
    }

    public virtual void TakeDamage()
    {
        currentMovementState = (int)MovementState.KnockBack;
        if (!ultDamaged) {
            body.TakeDamage(damage);
        }      
    }

    public virtual void OnCollide(Collider2D col)
    {

    }

    public virtual void EnableHitBox()
    {
        hitBox.enabled = true;
    }

    public virtual void DisableHitBox()
    {
        hitBox.enabled = false;
    }

    public virtual void EnableDamageBox()
    {
        damageBox.enabled = true;
    }

    public virtual void DisableDamageBox()
    {
        damageBox.enabled = false;
    }
}

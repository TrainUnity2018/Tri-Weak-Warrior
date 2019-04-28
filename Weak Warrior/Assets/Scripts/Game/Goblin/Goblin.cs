using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Monster
{

    public float moveSpeed;
    public float superMoveSpeed;
    public int damage;
    public bool spawnDirection;

    public float attackDelay;
    protected float attackDelayTimer;

    public GoblinDamageBox damageBox;
    public BoxCollider2D hitBox;

    public GoblinAnimationController animationController;

    public GameObject head;
    public GameObject body;
    public GameObject dieEffect;
    public GameObject shadow;

    public Vector2 bodySplashStartVelocity;
    protected Vector2 bodySplashVelocity;
    public Vector2 bodySplashDecelerate;

    public Vector2 headSplashStartVelocity;
    protected Vector2 headSplashVelocity;
    public Vector2 headSplashDecelerate;
    public float headSplashStartSpinningSpeed;
    protected float headSplashSpinningSpeed;
    public float headSplashSpinningDecelerate;

    public bool pause;
    public bool isSuper;

    public enum MovementState
    {
        Walk,
        Attack,
        AttackDelay,
        Die,
    }

    public int currentMovementState;

    // Use this for initialization
    void Start()
    {
        currentMovementState = (int)MovementState.Walk;
        animationController.SetMovementState(currentMovementState);
        pause = false;

		Setup(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            Walk();
            AttackDelayTiming();
        }

        OnDead();
		
        // if (!Popup.Instance.pauseDialog.activeSelf && !Popup.Instance.deadDiaglog.activeSelf)
        // {
        //     OnDead();
        // }
    }

    public override void Setup(bool direction)
    {
        animationController.Flip(direction);
        spawnDirection = direction;

        // if (model != null)
        // {
        //     isSuper = model.isSupper;
        // }
        // else
        // {

        // }
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

	public override void Attack()
	{
        if (currentMovementState == (int)MovementState.Walk)
        {
            currentMovementState = (int)MovementState.Attack;
            animationController.SetMovementState(currentMovementState);
        }
	}

	public override void AttackDelayTiming()
	{
        if (currentMovementState == (int)MovementState.AttackDelay)
        {
            attackDelayTimer += Time.deltaTime;
            if (attackDelayTimer >= attackDelay)
            {
                currentMovementState = (int)MovementState.Attack;
                animationController.SetMovementState(currentMovementState);
            }
        }
	}

	public override void AttackEnd()
	{
        if (currentMovementState != (int)MovementState.Die)
        {
            currentMovementState = (int)MovementState.AttackDelay;
            animationController.SetMovementState(currentMovementState);
            attackDelayTimer = 0;
        }
	}

    public override void TakeDamage(int playersMovementSate)
    {
        currentMovementState = (int)MovementState.Die;
        this.Pause();
        
		body.GetComponent<SpriteRenderer>().enabled = true;
        head.GetComponent<SpriteRenderer>().enabled = true;
        dieEffect.GetComponent<SpriteRenderer>().enabled = true;
        shadow.GetComponent<SpriteRenderer>().enabled = false;
		StartCoroutine(DieEffect());

        // audioSource.clip = hitSound;
        // audioSource.Play(0);

        headSplashSpinningSpeed = headSplashStartSpinningSpeed;
        headSplashVelocity = headSplashStartVelocity;
        bodySplashVelocity = bodySplashStartVelocity;
    }

    public override void OnDead()
    {
        if (body.GetComponent<MonsterBodyPart>().boundTouched && head.GetComponent<MonsterBodyPart>().boundTouched)
        {
            Destroy(gameObject);
            // EnemySpawnManager.Instance.EnemyKilled();
        }

        if (currentMovementState == (int)MovementState.Die)
        {
            DisableDamageBox();
            DisableHitBox();
			animationController.Disable();

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
            }
        }
    }

	public override void Pause()
	{

	}

	public override void UnPause()
	{
		
	}

	public override void EnableDamageBox()
	{
		damageBox.EnableDamageBox();
	}

	public override void DisableDamageBox()
	{
		damageBox.DisableDamageBox();
	}

	public override void EnableHitBox()
	{
		hitBox.enabled = true;
	}

	public override void DisableHitBox()
	{
		hitBox.enabled = false;
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

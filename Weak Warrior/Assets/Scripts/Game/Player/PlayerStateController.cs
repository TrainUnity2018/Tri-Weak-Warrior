using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateController : MonoSingleton<PlayerStateController>
{
    public enum MovementState
    {
        Idle,
        Attack,
        Skill,
        Die
    }

    public enum ArmorState
    {
        Full,
        Half,
        Naked
    }

    public int health;
    public bool beingDamaged;
    public float beingDamagedDuration;
    protected float beingDamagedDurationTimer;

    public int currentMovementState;
    public int currentArmorState;
    public bool currentDirection;

    public float attackDuration;
    protected float attackDurationTimer;
    public GameObject missText;

    public BoxCollider2D hitBox;
    public PlayerDamageBox damageBox;
    public PlayerDamageBox skillDamageBox;
    public float skillSpeed;
    public bool skillSide;

    public bool pause;

    public Transform skillLocationLeft;
    public Transform skillLocationRight;
    public Transform skillEndLocation;

    void Start()
    {
        this.Setup();
    }

    void Update()
    {
        if (!pause)
        {
            AttackDurationTiming();
            SkillDurationTiming();
            BeingDamagedDurationTiming();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {

    }

    public void Setup()
    {
        currentMovementState = (int)MovementState.Idle;
        PlayerAnimationController.Instance.SetMovementState(currentMovementState);

        currentArmorState = (int)ArmorState.Full;
        //currentArmorState = (int)ArmorState.Half;
        //currentArmorState = (int)ArmorState.Naked;       
        PlayerAnimationController.Instance.SetArmorState(currentArmorState);

        currentDirection = false;
        PlayerAnimationController.Instance.Flip(currentDirection);

        health = 3;
        beingDamaged = false;
        beingDamagedDurationTimer = 0;

        attackDurationTimer = 0;
        missText.GetComponent<Image>().enabled = false;

        pause = false;
    }

    public void Attack(bool direction)
    {
        if (currentMovementState == (int)MovementState.Idle)
        {
            currentMovementState = (int)MovementState.Attack;
            PlayerAnimationController.Instance.SetMovementState(currentMovementState);
            
            currentDirection = direction;
            PlayerAnimationController.Instance.Flip(currentDirection);
            
            attackDurationTimer = 0;
        }
    }

    public void AttackDurationTiming()
    {
        if (currentMovementState == (int)MovementState.Attack)
        {
            attackDurationTimer += Time.deltaTime;
            if (attackDurationTimer >= 0.25f)
            {
                EnebleMissText();
            }            
            if (attackDurationTimer >= attackDuration)
            {
                currentMovementState = (int)MovementState.Idle;
                PlayerAnimationController.Instance.SetMovementState(currentMovementState);

                DisableMissText();
            }            
        }
    }

    public void Skill()
    {
        beingDamaged = false;
        
        currentMovementState = (int)MovementState.Skill;
        PlayerAnimationController.Instance.SetMovementState(currentMovementState);
        
        skillSide = false;   
    }

    public void SkillDurationTiming()
    {
        if (currentMovementState == (int)MovementState.Skill)
        {
            if (!currentDirection)
            {
                if (!skillSide)
                {
                    if (transform.position.x < skillLocationRight.position.x)
                    {
                        transform.position += new Vector3(skillSpeed, 0) * Time.deltaTime;
                    }
                    else
                    {
                        transform.position = new Vector3(skillLocationLeft.position.x, transform.position.y, 0);
                        
                        skillSide = true;
                    }
                }
                else
                {
                    if (transform.position.x < skillEndLocation.position.x)
                    {
                        transform.position += new Vector3(skillSpeed, 0) * Time.deltaTime;
                    }
                    else
                    {
                        currentMovementState = (int)MovementState.Idle;
                        PlayerAnimationController.Instance.SetMovementState(currentMovementState);

                        beingDamaged = true;
                        beingDamagedDurationTimer = 0;
                        StartCoroutine(FlashSprite());
                        //DisableHitBox();

                        //DisableUltimateDamageBox();
                        // if (EnemySpawnManager.Instance.spawnedBosses.Count == 0)
                        // {
                        //     EnemySpawnManager.Instance.UnPause();
                        // }
                        // EnemySpawnManager.Instance.UltimateUnPause();
                        // EnemySpawnManager.Instance.AfterDash();
                        // UltimateCooldown.Instance.Setup();
                    }
                }
            }
            else
            {
                if (!skillSide)
                {
                    if (transform.position.x > skillLocationLeft.position.x)
                    {
                        transform.position -= new Vector3(skillSpeed, 0) * Time.deltaTime;
                    }
                    else
                    {
                        transform.position = new Vector3(skillLocationRight.position.x, transform.position.y, 0);
                        
                        skillSide = true;
                    }
                }
                else
                {
                    if (transform.position.x > skillEndLocation.position.x)
                    {
                        transform.position -= new Vector3(skillSpeed, 0) * Time.deltaTime;
                    }
                    else
                    {
                        currentMovementState = (int)MovementState.Idle;
                        PlayerAnimationController.Instance.SetMovementState(currentMovementState);
                        
                        beingDamaged = true;
                        beingDamagedDurationTimer = 0;
                        StartCoroutine(FlashSprite());
                        //DisableHitBox();

                        //DisableUltimateDamageBox();
                        // if (EnemySpawnManager.Instance.spawnedBosses.Count == 0)
                        // {
                        //     EnemySpawnManager.Instance.UnPause();
                        // }
                        // EnemySpawnManager.Instance.UltimateUnPause();
                        // EnemySpawnManager.Instance.AfterDash();
                        // UltimateCooldown.Instance.Setup();
                    }
                }

            }
        }
    }

    public void BeingDamagedDurationTiming()
    {
        if (beingDamaged && currentMovementState != (int)MovementState.Skill)
        {
            beingDamagedDurationTimer += Time.deltaTime;
            if (beingDamagedDurationTimer >= beingDamagedDuration)
            {
                beingDamaged = false;
                
                //EnableHitBox();
                StopAllCoroutines();
                this.gameObject.GetComponent<Image>().enabled = true;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        
        currentMovementState = (int)MovementState.Idle;
        PlayerAnimationController.Instance.SetMovementState(currentMovementState);
        
        beingDamaged = true;
        beingDamagedDurationTimer = 0;
        StartCoroutine(FlashSprite());
        //DisableHitBox();
        
        if (health <= 0)
        {
            // audioSource.clip = dieSound;
            // audioSource.Play(0);
            
            health = 0;

            // Popup.Instance.EnableDeadDialog(EnemySpawnManager.Instance.enemyLevelID, EnemySpawnManager.Instance.enemyKilled);
            // EnemySpawnManager.Instance.Pause();
            
            //this.gameObject.SetActive(false);
        }
        if (health == 3)
        {
            // audioSource.clip = hitSound;
            // audioSource.Play(0);
            
            currentArmorState = 0;
            PlayerAnimationController.Instance.SetArmorState(currentArmorState);
        }
        else if (health == 2)
        {
            // audioSource.clip = hitSound;
            // audioSource.Play(0);

            currentArmorState = 1;
            PlayerAnimationController.Instance.SetArmorState(currentArmorState);
        }
        else if (health == 1)
        {
            // audioSource.clip = hitSound;
            // audioSource.Play(0);

            currentArmorState = 2;
            PlayerAnimationController.Instance.SetArmorState(currentArmorState);
        }
    }

    public void EnebleMissText()
    {
        this.missText.GetComponent<Image>().enabled = true;
    }

    public void DisableMissText()
    {
        this.missText.GetComponent<Image>().enabled = false;
    }

    IEnumerator FlashSprite()
    {
        while (true)
        {
            this.gameObject.GetComponent<Image>().enabled = false;
            yield return new WaitForSeconds(.05f);
            this.gameObject.GetComponent<Image>().enabled = true;
            yield return new WaitForSeconds(.05f);
        }
    }
}

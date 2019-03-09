using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStateControl : MonoSingleton<PlayerStateControl>
{
    public enum MovementState
    {
        Idle,
        Slash,
        Dash,
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

    public float slashDuration;
    protected float slashDurationTimer;

    public BoxCollider2D hitBox;
    public Player_DamageBox damageBox;
    public Player_DamageBox ultimateDamageBox;
    public float ultimateSpeed;
    public bool ultimateSide;

    public bool pause;

    public GameObject slashLeftButton;
    public GameObject slashRightButton;

    public Transform ultimateLocationLeft;
    public Transform ultimateLocationRight;
    public Transform ultimateEndLocation;

    public enum State
    {
        Active,
        Inactive
    }

    // Use this for initialization
    void Start()
    {
        this.Setup();
    }

    // Update is called once per frame
    void Update()
    {
        SlashDurationTiming();
        DashDurationTiming();
        BeingDamagedDurationTiming();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        OnCollide(col);
    }

    public void Setup()
    {
        currentMovementState = (int)MovementState.Idle;
        currentArmorState = (int)ArmorState.Full;
        health = 3;
        beingDamaged = false;
        pause = false;
        currentDirection = false;
        // currentArmorState = (int)ArmorState.Half;
        // currentArmorState = (int)ArmorState.Naked;
        PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
        PlayerAnimationControl.Instance.SetArmorState(currentArmorState);
        //transform.position = new Vector3(ultimateLocationLeft.position.x, ultimateLocationLeft.position.y, 0);
    }

    public void Slash(bool direction)
    {
        if (currentMovementState == (int)MovementState.Idle)
        {
            currentMovementState = (int)MovementState.Slash;
            PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
            PlayerAnimationControl.Instance.Slash(direction);
            currentDirection = direction;
            slashDurationTimer = 0;
        }
    }

    public void SlashDurationTiming()
    {
        if (currentMovementState == (int)MovementState.Slash)
        {
            slashDurationTimer += Time.deltaTime;
            if (slashDurationTimer >= slashDuration)
            {
                currentMovementState = (int)MovementState.Idle;
                PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
            }
        }
    }

    public void BeingDamagedDurationTiming()
    {
        if (beingDamaged)
        {
            beingDamagedDurationTimer += Time.deltaTime;
            if (beingDamagedDurationTimer >= beingDamagedDuration)
            {
                beingDamaged = false;
                PlayerAnimationControl.Instance.SetBeingDamagedState(beingDamaged);
            }
        }
        else
        {
            beingDamagedDurationTimer = 0;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        beingDamaged = true;
        PlayerAnimationControl.Instance.SetBeingDamagedState(beingDamaged);
        currentMovementState = (int)MovementState.Idle;
        PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
        if (health <= 0)
        {

            Popup.Instance.EnableDeadDialog(EnemySpawnManager.Instance.enemyLevelID, EnemySpawnManager.Instance.enemyKilled);
            EnemySpawnManager.Instance.Pause();
            slashLeftButton.SetActive(false);
            slashRightButton.SetActive(false);
            health = 0;
            currentMovementState = (int)MovementState.Die;
            PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
        }
        if (health == 3)
        {
            currentArmorState = 0;
            PlayerAnimationControl.Instance.SetArmorState(currentArmorState);
        }
        else if (health == 2)
        {
            currentArmorState = 1;
            PlayerAnimationControl.Instance.SetArmorState(currentArmorState);
        }
        else if (health == 1)
        {
            currentArmorState = 2;
            PlayerAnimationControl.Instance.SetArmorState(currentArmorState);
        }
    }

    public void Dash()
    {
        if (currentMovementState == (int)MovementState.Idle)
        {
            currentMovementState = (int)MovementState.Dash;
            PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
            ultimateSide = false;
        }
    }
    public void DashDurationTiming()
    {
        if (currentMovementState == (int)MovementState.Dash)
        {
            if (!currentDirection)
            {
                if (!ultimateSide) {
                    if (transform.position.x < ultimateLocationRight.position.x)
                    {
                        transform.position += new Vector3(ultimateSpeed, 0) * Time.deltaTime;
                    }
                    else
                    {
                        transform.position = new Vector3(ultimateLocationLeft.position.x, transform.position.y, 0);
                        ultimateSide = true;
                    }
                }
                else {
                    if (transform.position.x < ultimateEndLocation.position.x)
                    {
                        transform.position += new Vector3(ultimateSpeed, 0) * Time.deltaTime;
                    }
                    else
                    {
                        currentMovementState = (int)MovementState.Idle;
                        PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
                        EnemySpawnManager.Instance.UnPause();
                        UltimateCooldown.Instance.Setup();
                    }
                }               
            }
            else
            {
                if (!ultimateSide)
                {
                    if (transform.position.x > ultimateLocationLeft.position.x)
                    {
                        transform.position -= new Vector3(ultimateSpeed, 0) * Time.deltaTime;
                    }
                    else
                    {
                        transform.position = new Vector3(ultimateLocationRight.position.x, transform.position.y, 0);
                        ultimateSide = true;
                    }
                }
                else
                {
                    if (transform.position.x > ultimateEndLocation.position.x)
                    {
                        transform.position -= new Vector3(ultimateSpeed, 0) * Time.deltaTime;
                    }
                    else
                    {
                        currentMovementState = (int)MovementState.Idle;
                        PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
                        EnemySpawnManager.Instance.UnPause();
                        UltimateCooldown.Instance.Setup();
                    }
                }
            }

            //this.gameObject.transform.position += new Vector3(5f, 0) * Time.deltaTime;

        }
    }

    public void EnableHitBox()
    {
        hitBox.enabled = true;
    }

    public void DisableHitBox()
    {
        hitBox.enabled = false;
    }

    public void EnableDamageBox()
    {
        damageBox.EnableDamageBox();
    }

    public void EnableUltimateDamageBox()
    {
        ultimateDamageBox.EnableDamageBox();
    }

    public void DisableDamageBox()
    {
        damageBox.DisableDamageBox();
    }

    public void SetIdleState()
    {
        currentMovementState = (int)MovementState.Idle;
        PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
    }

    public void OnCollide(Collider2D col)
    {

    }

    public void Pause()
    {
        this.pause = true;
        DisableDamageBox();
        DisableHitBox();
    }

    public void UnPause()
    {
        this.pause = false;
        PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
    }
}

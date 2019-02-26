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

    public float slashDuration;
    protected float slashDurationTimer;

    public BoxCollider2D hitBox;
    public Player_DamageBox damageBox;

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
        // currentArmorState = (int)ArmorState.Half;
        // currentArmorState = (int)ArmorState.Naked;
        PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
        PlayerAnimationControl.Instance.SetArmorState(currentArmorState);
    }

    public void Slash(bool direction)
    {
        if (currentMovementState == (int)MovementState.Idle)
        {
            currentMovementState = (int)MovementState.Slash;
            PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
            PlayerAnimationControl.Instance.Slash(direction);
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
            health = 0;
            Popup.Instance.Enable();
            Popup.Instance.LastestEnemyShow(EnemySpawnManager.Instance.lastestEnemySpawed);
            EnemySpawnManager.Instance.Pause();
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


}

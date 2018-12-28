using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStateControl : MonoBehaviour {

    public GameObject player;

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
    public int currentMovementState;
    public int currentArmorState;

    public float slashDuration;
    protected float slashDurationTimer;

    public enum State
    {
        Active,
        Inactive
    }
	
    // Use this for initialization
	void Start () {
        currentMovementState = (int)MovementState.Idle;
        currentArmorState = (int)ArmorState.Full;
        player.GetComponent<PlayerAnimationControl>().SetMovementState(currentMovementState);
        player.GetComponent<PlayerAnimationControl>().SetArmorState(currentArmorState);
    }
	
	// Update is called once per frame
	void Update () {
        SlashDurationTiming();
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        OnCollide(col);
    }

    public virtual void Slash(bool direction)
    {
        if (currentMovementState == (int)MovementState.Idle)
        {
            currentMovementState = (int)MovementState.Slash;
            player.GetComponent<PlayerAnimationControl>().SetMovementState(currentMovementState);
            player.GetComponent<PlayerAnimationControl>().Slash(direction);
            slashDurationTimer = 0;
        }
    }

    public virtual void SlashDurationTiming()
    {
        if (currentMovementState == (int)MovementState.Slash)
        {
            slashDurationTimer += Time.deltaTime;
            if (slashDurationTimer >= slashDuration)
            {
                currentMovementState = (int)MovementState.Idle;
                player.GetComponent<PlayerAnimationControl>().SetMovementState(currentMovementState);
            }
        }
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            health = 0;
        if (health == 3)
        {
            currentArmorState = 0;
            player.GetComponent<PlayerAnimationControl>().SetArmorState(currentArmorState);
        }
        else if (health == 2)
        {
            currentArmorState = 1;
            player.GetComponent<PlayerAnimationControl>().SetArmorState(currentArmorState);
        }
        else if (currentArmorState == 1)
        {
            currentArmorState = 2;
            player.GetComponent<PlayerAnimationControl>().SetArmorState(currentArmorState);
        }
    }

    public virtual void OnCollide(Collider2D col)
    {
        
    }
}

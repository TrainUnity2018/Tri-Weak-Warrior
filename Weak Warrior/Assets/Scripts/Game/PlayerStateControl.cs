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

    public int health;
    public int currentState;

    public float slashDuration;
    protected float slashDurationTimer;

    public enum State
    {
        Active,
        Inactive
    }
	
    // Use this for initialization
	void Start () {
        currentState = (int)MovementState.Idle;
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
        if (currentState == (int)MovementState.Idle)
        {
            currentState = (int)MovementState.Slash;
            player.GetComponent<PlayerAnimationControl>().SetState(currentState);
            player.GetComponent<PlayerAnimationControl>().Slash(direction);
            slashDurationTimer = 0;
        }
    }

    public virtual void SlashDurationTiming()
    {
        if (currentState == (int)MovementState.Slash)
        {
            slashDurationTimer += Time.deltaTime;
            if (slashDurationTimer >= slashDuration)
            {
                currentState = (int)MovementState.Idle;
                player.GetComponent<PlayerAnimationControl>().SetState(currentState);
            }
        }
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            health = 0;
    }

    public virtual void OnCollide(Collider2D col)
    {
        
    }
}

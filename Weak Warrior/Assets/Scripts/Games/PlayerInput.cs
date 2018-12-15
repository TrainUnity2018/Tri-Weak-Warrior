using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public GameObject player;

    public float attackDelay;
    private float attackDelayTimer;

    public float ultimateCooldown;
    private float ultimateCooldownTimer;
	
    // Use this for initialization
	void Start () {
        
        ultimateCooldownTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        InputHandle();
	}

    public virtual void InputHandle()
    {
        attackDelayTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && attackDelayTimer >= attackDelay)
        {
            attackDelayTimer = 0;
            player.GetComponent<Player>().SlashLeft();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && attackDelayTimer >= attackDelay)
        {
            attackDelayTimer = 0;
            player.GetComponent<Player>().SlashRight();
        }

        ultimateCooldownTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && ultimateCooldownTimer >= ultimateCooldown)
        {
            ultimateCooldownTimer = 0;
            player.GetComponent<Player>().UltimateCast();
        }
    }
}

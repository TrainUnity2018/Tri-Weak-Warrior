using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DamageBox : DamageBox {

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnCollide(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            PlayerStateControl.Instance.currentMovementState = (int)PlayerStateControl.MovementState.Idle;
            PlayerAnimationControl.Instance.SetMovementState(PlayerStateControl.Instance.currentMovementState);

            col.gameObject.GetComponent<GoblinSwordman>().TakeDamage();
        }
    }
}

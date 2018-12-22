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
            actor.GetComponent<PlayerStateControl>().currentState = 0;
            actor.GetComponent<PlayerAnimationControl>().SetState(actor.GetComponent<PlayerStateControl>().currentState);
        }
    }
}

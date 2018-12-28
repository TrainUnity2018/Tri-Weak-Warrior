using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSwordman_DamageBox : DamageBox {

	// Use this for initialization
	void Start () {
        damage = actor.GetComponent<GoblinSwordman>().damage;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnCollide(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerStateControl>().TakeDamage(damage);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSwordman_DamageBox : DamageBox {

	// Use this for initialization
	void Start () {
        if (actor.GetComponent<GoblinSwordman>() != null)
            damage = actor.GetComponent<GoblinSwordman>().damage;
        if (actor.GetComponent<DarkTree_Arm>() != null)
            damage = actor.GetComponent<DarkTree_Arm>().damage;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnCollide(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            PlayerStateControl.Instance.TakeDamage(damage);
        }
    }
}

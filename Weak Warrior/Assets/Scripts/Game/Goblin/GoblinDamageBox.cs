using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinDamageBox : DamageBox
{
    // Use this for initialization
    void Start()
    {
        if (actor.GetComponent<Goblin>() != null)
			damage = actor.GetComponent<Goblin>().damage;
        // if (actor.GetComponent<DarkTree_Arm>() != null)
        //     damage = actor.GetComponent<DarkTree_Arm>().damage;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnCollide(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {            
            PlayerStateController.Instance.TakeDamage(damage);
        }
    }
}

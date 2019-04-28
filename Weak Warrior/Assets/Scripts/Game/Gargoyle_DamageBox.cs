using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle_DamageBox : DamageBox {

    // Use this for initialization
    void Start()
    {
        if (actor.GetComponent<GoblinSwordman>() != null)
            damage = actor.GetComponent<GoblinSwordman>().damage;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnCollide(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            PlayerStateControl.Instance.TakeDamage(damage);
			DisableDamageBox();
        }
    }
}

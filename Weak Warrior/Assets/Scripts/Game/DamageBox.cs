using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBox : MonoBehaviour {

    public GameObject actor;
    public BoxCollider2D damageBox;
    public int damage;

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void EnableDamageBox()
    {
        damageBox.enabled = true;
    }

    public virtual void DisableDamageBox()
    {
        damageBox.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        OnCollide(col);
    }

    public virtual void OnCollide(Collider2D col)
    {

    }
}

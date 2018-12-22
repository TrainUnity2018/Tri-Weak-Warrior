using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBox : MonoBehaviour {

    public GameObject actor;
    public int damage;

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        OnCollide(col);
    }

    public virtual void OnCollide(Collider2D col)
    {

    }
}

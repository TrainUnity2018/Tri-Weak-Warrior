using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectBox : DetectBox {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnCollide(Collider2D col) {
		if (col.gameObject.tag == "Bound")
			Debug.Log("hit");
	}
}

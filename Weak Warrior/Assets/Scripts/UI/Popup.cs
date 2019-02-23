using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoSingleton<Popup> {

	// Use this for initialization
	void Start () {
        Disable();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Disable() {
        this.gameObject.SetActive(false);
	}

	public void Enable() {
		this.gameObject.SetActive(true);
	}
}

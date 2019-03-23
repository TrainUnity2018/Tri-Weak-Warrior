using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu_ButtonContinue : Button {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void onClick() {
		Popup.Instance.DisableStartMenu();
	}
}

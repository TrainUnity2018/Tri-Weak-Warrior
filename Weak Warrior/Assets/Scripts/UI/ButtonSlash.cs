using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSlash : Button {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onClickLeft()
	{
		PlayerInput.Instance.SlashLeft();
	}

	public void onClickRight()
	{
        PlayerInput.Instance.SlashRight();
	}
}

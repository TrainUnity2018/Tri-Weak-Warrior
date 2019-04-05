using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Text : MonoSingleton<UI_Text> {

	public GameObject missedText;
	
	// Use this for initialization
	void Start () {
        missedText.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void EnableMissedText() {
		missedText.SetActive(true);
	}

	public void DisableMissedText() {
        missedText.SetActive(false);
	}
}

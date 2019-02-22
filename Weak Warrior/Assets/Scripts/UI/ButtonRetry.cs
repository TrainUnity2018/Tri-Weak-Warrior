using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonRetry : Button {

    public GameObject deadDialog;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void onClick() {
        // PlayerStateControl.Instance.Setup();
        // EnemySpawnManager.Instance.Setup();
        // deadDialog.SetActive(false);
        SceneManager.LoadScene("SampleScene");
	}
}

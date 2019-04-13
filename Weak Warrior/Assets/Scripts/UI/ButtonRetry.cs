using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonRetry : Button {

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void onClick() {
		Popup.Instance.DisableDeadDialog();
		PlayerStateControl.Instance.Revive();
		EnemySpawnManager.Instance.UnPause();
		EnemySpawnManager.Instance.KillFirstEnemy();
	}
}

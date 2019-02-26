using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoSingleton<Popup> {

    public Transform lastestEnemyLocation;
    
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

    public void LastestEnemyShow(GoblinSwordman enemy)
    {
        GoblinSwordman lastestEnemy = Instantiate(enemy) as GoblinSwordman;
        lastestEnemy.transform.position = new Vector3(this.lastestEnemyLocation.position.x, this.lastestEnemyLocation.position.y);
        lastestEnemy.Setup(true, null);
    }
}

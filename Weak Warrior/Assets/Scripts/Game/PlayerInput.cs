using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public GameObject player;

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        InputHandle();
	}

    public virtual void InputHandle()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            player.GetComponent<PlayerStateControl>().Slash(true);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            player.GetComponent<PlayerStateControl>().Slash(false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }
}

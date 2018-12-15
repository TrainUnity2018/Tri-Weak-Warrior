using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public enum PlayerState
    {
        Idle = 0,
        Slash = 1,
        Die = 2,
        UltimateCast = 3
    }
    public PlayerState state;
    public Animator animation;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    [ContextMenu("test")]
    public virtual void SlashLeft()
    {
        animation.SetTrigger(state.ToString());
        //animation.Play(state.ToString());
    }
    public void ChangeState()
    {
        this.state = PlayerState.Die;
        this.SlashLeft();
    }
    public virtual void SlashRight()
    {

    }

    public virtual void UltimateCast()
    {

    }

    public virtual void Die()
    {

    }

    
}

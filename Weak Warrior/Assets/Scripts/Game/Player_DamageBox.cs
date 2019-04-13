using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DamageBox : DamageBox
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnCollide(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (col.gameObject.GetComponent<GoblinSwordman>() != null)
                col.gameObject.GetComponent<GoblinSwordman>().TakeDamage(PlayerStateControl.Instance.currentMovementState);
            if (col.gameObject.GetComponent<DarkTree_Arm>() != null)
                col.gameObject.GetComponent<DarkTree_Arm>().TakeDamage();

            if (PlayerStateControl.Instance.currentMovementState == (int)PlayerStateControl.MovementState.Slash)
            {
                PlayerStateControl.Instance.currentMovementState = (int)PlayerStateControl.MovementState.Idle;
                PlayerAnimationControl.Instance.SetMovementState(PlayerStateControl.Instance.currentMovementState);
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (col.gameObject.GetComponent<GoblinSwordman>() != null)
                col.gameObject.GetComponent<GoblinSwordman>().TakeDamage(PlayerStateControl.Instance.currentMovementState);
            if (col.gameObject.GetComponent<DarkTree_Arm>() != null)
                col.gameObject.GetComponent<DarkTree_Arm>().TakeDamage();

            if (PlayerStateControl.Instance.currentMovementState == (int)PlayerStateControl.MovementState.Slash)
            {
                PlayerStateControl.Instance.currentMovementState = (int)PlayerStateControl.MovementState.Idle;
                PlayerAnimationControl.Instance.SetMovementState(PlayerStateControl.Instance.currentMovementState);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (col.gameObject.GetComponent<GoblinSwordman>() != null)
                col.gameObject.GetComponent<GoblinSwordman>().TakeDamage(PlayerStateControl.Instance.currentMovementState);
            if (col.gameObject.GetComponent<DarkTree_Arm>() != null)
                col.gameObject.GetComponent<DarkTree_Arm>().TakeDamage();

            if (PlayerStateControl.Instance.currentMovementState == (int)PlayerStateControl.MovementState.Slash)
            {
                PlayerStateControl.Instance.currentMovementState = (int)PlayerStateControl.MovementState.Idle;
                PlayerAnimationControl.Instance.SetMovementState(PlayerStateControl.Instance.currentMovementState);
            }
        }
    }
}

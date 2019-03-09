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
            if (PlayerStateControl.Instance.currentMovementState == (int)PlayerStateControl.MovementState.Slash)
            {
                if (col.gameObject.GetComponent<GoblinSwordman>() != null)
                    col.gameObject.GetComponent<GoblinSwordman>().TakeDamage();
                if (col.gameObject.GetComponent<DarkTree_Arm>() != null)
                    col.gameObject.GetComponent<DarkTree_Arm>().TakeDamage();
                PlayerStateControl.Instance.currentMovementState = (int)PlayerStateControl.MovementState.Idle;
                PlayerAnimationControl.Instance.SetMovementState(PlayerStateControl.Instance.currentMovementState);
            }
            else if (PlayerStateControl.Instance.currentMovementState == (int)PlayerStateControl.MovementState.Dash)
            {
                if (col.gameObject.GetComponent<GoblinSwordman>() != null)
                    Destroy(col.gameObject);
                if (col.gameObject.GetComponent<DarkTree_Arm>() != null)
                    col.gameObject.GetComponent<DarkTree_Arm>().TakeDamage();
            }
        }
    }
}

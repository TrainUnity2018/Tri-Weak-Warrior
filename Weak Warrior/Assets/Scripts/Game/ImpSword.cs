using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpSword : GoblinSwordman {

    public float jumpDecelerate;
    public float startJumpSpeed;
    protected Vector3 jumpSpeed;
    public float fallAccelerate;
    protected Vector3 fallSpeed;
    public bool isJump;

    void Start()
    {

    }

    public override void Setup(bool direction, ModelLevel model = null)
    {
        isJump = true;
        jumpSpeed = new Vector3(0, startJumpSpeed);
        fallSpeed = new Vector3(0, 0);
        currentMovementState = (int)MovementState.Walk;
        animator.SetInteger("State", currentMovementState);
        Flip(direction);
        spawnDirection = direction;
    }

    public override void Walk()
    {
        if (currentMovementState == (int)MovementState.Walk)
        {
            if (spawnDirection == true)
            {
                gameObject.transform.position += new Vector3(moveSpeed, 0) * Time.deltaTime;
            }
            else
            {
                gameObject.transform.position -= new Vector3(moveSpeed, 0) * Time.deltaTime;
            }

            if (isJump)
            {
                if (jumpSpeed.y <= 0)
                {
                    isJump = false;
                    fallSpeed = new Vector3(0, 0);
                }
                else
                {
                    jumpSpeed -= new Vector3(0, jumpDecelerate);
                    gameObject.transform.position += jumpSpeed * Time.deltaTime;
                }
            }
            else
            {
                fallSpeed += new Vector3(0, fallAccelerate);
                gameObject.transform.position -= fallSpeed * Time.deltaTime;
            }
        }
    }

    public override void OnCollide(Collider2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isJump = true;
            jumpSpeed = new Vector3(0, startJumpSpeed);
        }
    }
}

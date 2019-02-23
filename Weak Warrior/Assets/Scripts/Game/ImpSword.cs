using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpSword : GoblinSwordman {

    public float jumpDecelerate;
    public float startJumpSpeed;
    protected float jumpSpeed;
    public float fallAccelerate;
    public float startFallSpeed;
    protected float fallSpeed;
    public bool isJump;

    void Start()
    {
        currentMovementState = (int)MovementState.Walk;
        //animator.SetInteger("State", currentMovementState);

        isJump = true;
        jumpSpeed = startJumpSpeed;
        fallSpeed = startFallSpeed;
    }

    void Update()
    {
        if (!pause)
        {
            Walk();
        }
    }

    public override void Setup(bool direction, ModelLevel model = null)
    {
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
                if (jumpSpeed <= 0)
                {
                    isJump = false;
                    fallSpeed = startFallSpeed;
                }
                else
                {
                    jumpSpeed -= jumpDecelerate * Time.deltaTime;
                    gameObject.transform.position += new Vector3(0, jumpSpeed)  * Time.deltaTime;
                }
            }
            else
            {
                fallSpeed += fallAccelerate * Time.deltaTime;
                gameObject.transform.position -= new Vector3(0, fallSpeed) * Time.deltaTime;
            }
        }
    }

    public override void OnCollide(Collider2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            isJump = true;
            jumpSpeed = startJumpSpeed;
        }
    }
}

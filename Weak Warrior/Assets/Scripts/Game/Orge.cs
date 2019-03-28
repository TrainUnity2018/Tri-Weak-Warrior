﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orge : GoblinSwordman {

	public int health;

	// Use this for initialization
	void Start()
    {
        EnableHitBox();
        currentMovementState = (int)MovementState.Walk;
        animator.SetInteger("State", currentMovementState);
        pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            Walk();
            SlashDelayTiming();
        }
        if (!Popup.Instance.pauseDialog.activeSelf && !Popup.Instance.deadDiaglog.activeSelf)
        {
            OnDead();
        }
    }

	public override void TakeDamage()
    {
        if (PlayerStateControl.Instance.currentMovementState == (int)PlayerStateControl.MovementState.Slash) {
            health -= 1;
        }            
        else if (PlayerStateControl.Instance.currentMovementState == (int)PlayerStateControl.MovementState.Dash) {
            health -= 4;
        }
        
        if (health > 0)
        {
            EnableHitBox();
        }
        else if (health <= 0)
        {
            currentMovementState = (int)MovementState.Die;
            pause = true;
            body.GetComponent<SpriteRenderer>().enabled = true;
            head.GetComponent<SpriteRenderer>().enabled = true;

            headSplashSpinningSpeed = headSplashStartSpinningSpeed;
            headSplashVelocity = headSplashStartVelocity;
            bodySplashVelocity = bodySplashStartVelocity;
        }
    }

    public override void OnDead()
    {
        if (body.GetComponent<GoblinSwordman_BodyPart>().boundTouched && head.GetComponent<GoblinSwordman_BodyPart>().boundTouched)
        {
            Destroy(gameObject);
            EnemySpawnManager.Instance.EnemyKilled();
        }

        if (currentMovementState == (int)MovementState.Die)
        {
            DisableDamageBox();
            DisableHitBox();
            if (spawnDirection)
            {
                if (headSplashVelocity.x > 0)
                {
                    head.gameObject.transform.position += new Vector3(headSplashVelocity.x, 0) * Time.deltaTime;
                }
                head.gameObject.transform.position += new Vector3(0, headSplashVelocity.y) * Time.deltaTime;
                headSplashVelocity = new Vector2(headSplashVelocity.x - headSplashDecelerate.x * Time.deltaTime, headSplashVelocity.y - headSplashDecelerate.y * Time.deltaTime);
                headSplashDecelerate = new Vector2(headSplashDecelerate.x - Time.deltaTime * 10, headSplashDecelerate.y);

                if (headSplashSpinningSpeed > 0)
                {
                    float degreesPerSec = headSplashSpinningSpeed;
                    float rotAmount = degreesPerSec * Time.deltaTime;
                    float curRot = head.gameObject.transform.localRotation.eulerAngles.z;
                    head.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curRot + rotAmount));
                    headSplashSpinningSpeed -= headSplashSpinningDecelerate * Time.deltaTime;
                }

                if (bodySplashVelocity.x > 0)
                {
                    body.gameObject.transform.position -= new Vector3(bodySplashVelocity.x, 0) * Time.deltaTime;
                }
                body.gameObject.transform.position += new Vector3(0, bodySplashVelocity.y) * Time.deltaTime;
                bodySplashVelocity = new Vector2(bodySplashVelocity.x - bodySplashDecelerate.x * Time.deltaTime, bodySplashVelocity.y - bodySplashDecelerate.y * Time.deltaTime);
                bodySplashDecelerate = new Vector2(bodySplashDecelerate.x - Time.deltaTime * 10, bodySplashDecelerate.y);

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                if (headSplashVelocity.x > 0)
                {
                    head.gameObject.transform.position -= new Vector3(headSplashVelocity.x, 0) * Time.deltaTime;
                }
                head.gameObject.transform.position += new Vector3(0, headSplashVelocity.y) * Time.deltaTime;
                headSplashVelocity = new Vector2(headSplashVelocity.x - headSplashDecelerate.x * Time.deltaTime, headSplashVelocity.y - headSplashDecelerate.y * Time.deltaTime);
                headSplashDecelerate = new Vector2(headSplashDecelerate.x - Time.deltaTime * 10, headSplashDecelerate.y);

                if (headSplashSpinningSpeed > 0)
                {
                    float degreesPerSec = headSplashSpinningSpeed;
                    float rotAmount = degreesPerSec * Time.deltaTime;
                    float curRot = head.gameObject.transform.localRotation.eulerAngles.z;
                    head.gameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, curRot + rotAmount));
                    headSplashSpinningSpeed -= headSplashSpinningDecelerate * Time.deltaTime;
                }

                if (bodySplashVelocity.x > 0)
                {
                    body.gameObject.transform.position += new Vector3(bodySplashVelocity.x, 0) * Time.deltaTime;
                }
                body.gameObject.transform.position += new Vector3(0, bodySplashVelocity.y) * Time.deltaTime;
                bodySplashVelocity = new Vector2(bodySplashVelocity.x - bodySplashDecelerate.x * Time.deltaTime, bodySplashVelocity.y - bodySplashDecelerate.y * Time.deltaTime);
                bodySplashDecelerate = new Vector2(bodySplashDecelerate.x - Time.deltaTime * 10, bodySplashDecelerate.y);

                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
}

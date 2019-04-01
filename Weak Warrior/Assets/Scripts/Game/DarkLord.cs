using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkLord : DarkTree {

	public DarkLord_Head head;

    // Use this for initialization
    void Start()
    {
        EnemySpawnManager.Instance.Pause();
        pause = false;
        // this.currentIndex = 0;
        // this.attackDelayTimer = 0;
        // currentLevel = new ModelLevel_Boss();
        // currentMovementState = (int)MovementState.Spawning;
        // this.transform.position = new Vector3(spawnLocation.position.x, spawnLocation.position.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            Spawning();
            if (currentMovementState == (int)MovementState.Attack)
            {
                if (this.currentLevel != null)
                {
                    this.attackDelayTimer += Time.deltaTime;
                    if (this.attackDelayTimer >= this.currentLevel.attackDelay)
                    {
                        this.Attack();
                    }
                }
            }
        }
        if (!Popup.Instance.pauseDialog.activeSelf && !Popup.Instance.deadDiaglog.activeSelf)
        {
            OnDead();
        }
    }

	public override void Spawning() {
        if (currentMovementState == (int)MovementState.Spawning)
        {
            if ((Mathf.Abs(transform.position.x - attackLocation.position.x) > 0.01f) || (Mathf.Abs(transform.position.y - attackLocation.position.y) > 0.01f))
            {
                Vector3 moveVector = (attackLocation.position - transform.position).normalized;
                transform.position += moveVector * spawnSpeed * Time.deltaTime;
            }
            else
            {
                transform.position = new Vector3(attackLocation.position.x, attackLocation.position.y);
                currentMovementState = (int)MovementState.Attack;
                leftArm.Setup();
                rightArm.Setup();
				head.Setup();
            }
        }
	}

	public override void Attack() {
        //if (this.currentIndex < this.levels.Count)
        if (this.currentIndex < 50)
        {
            //this.currentLevel = this.levels[this.currentIndex];
            if (this.currentLevel != null)
            {
                //int location = currentLevel.armID;
                int location = (int)Random.Range(1,4);
                if (location == 1)
                {
                    leftArm.PrepareAttack();
                    this.attackDelayTimer = 0;
                    this.currentIndex++;

                }
                else if (location == 2)
                {
                    rightArm.PrepareAttack();
                    this.attackDelayTimer = 0;
                    this.currentIndex++;
                }
				else
				{
					head.PrepareAttack();
                    this.attackDelayTimer = 0;
                    this.currentIndex++;
				}
            }
        }
        else
        {
            currentIndex = 0;
        }
	}

	public override void TakeDamage(int damage) {
        if (health - damage <= 0)
        {
            health = 0;
            currentMovementState = (int)DarkTree.MovementState.Die;
            Destroy(leftArm.gameObject);
            Destroy(rightArm.gameObject);
			Destroy(head.gameObject);
        }
        else
        {
            health -= damage;
        }
	}
}

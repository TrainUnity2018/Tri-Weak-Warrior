using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModelLevel_Boss
{
    public int armID = -1;
    public float attackDelay = 0.0f;
    public bool isSupper = false;
    public float attackSpeed = 1;
    public ModelLevel_Boss()
    {
        this.armID = 0;
        this.attackDelay = 5.0f;
    }

    public ModelLevel_Boss(int armID, float attackDelay)
    {
        this.armID = armID;
        this.attackDelay = attackDelay;
    }

    public ModelLevel_Boss(int armID)
    {
        this.armID = armID;
        this.attackDelay = 5.0f;
    }
}
public class DarkTree : MonoBehaviour
{

    public DarkTree_Arm leftArm;
    public DarkTree_Arm rightArm;

    public int health;

    public Transform spawnLocation;
    public Transform attackLocation;
    public float spawnSpeed;

    public List<ModelLevel_Boss> levels;
    private ModelLevel_Boss currentLevel = null;
    private int currentIndex = 0;
    private float attackDelayTimer = 0;

    public enum MovementState
    {
        Spawning,
        Attack,
    }
    public int currentMovementState;

    // Use this for initialization
    void Start()
    {
        // this.currentIndex = 0;
        // this.attackDelayTimer = 0;
        // currentLevel = new ModelLevel_Boss();
        // currentMovementState = (int)MovementState.Spawning;
        // this.transform.position = new Vector3(spawnLocation.position.x, spawnLocation.position.y, 0);
    }

    // Update is called once per frame
    void Update()
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

    public virtual void Setup(Transform spawnLocation, Transform attackLocation) {
        this.spawnLocation = spawnLocation;
        this.attackLocation = attackLocation;
        this.currentIndex = 0;
        this.attackDelayTimer = 0;
        currentLevel = new ModelLevel_Boss();
        currentMovementState = (int)MovementState.Spawning;
        this.transform.position = new Vector3(spawnLocation.position.x, spawnLocation.position.y, 0);
    }

    public void Spawning()
    {
        if (currentMovementState == (int)MovementState.Spawning)
        {
            if ((Mathf.Abs(transform.position.x - attackLocation.position.x) > 0.01f) || (Mathf.Abs(transform.position.y - attackLocation.position.y) > 0.01f))
            {
                Vector3 moveVector = (attackLocation.position - transform.position).normalized;
                transform.position += moveVector * spawnSpeed * Time.deltaTime;
            }
            else
            {
                currentMovementState = (int)MovementState.Attack;
                leftArm.Setup();
                rightArm.Setup();
            }
        }
    }
    private void Attack()
    {
        //if (this.currentIndex < this.levels.Count)
        if (this.currentIndex < 50)
        {
            //this.currentLevel = this.levels[this.currentIndex];
            if (this.currentLevel != null)
            {
                //int location = currentLevel.armID;
                int location = Random.Range(0, 2);
                if (location == 1)
                {
                    leftArm.PrepareAttack();
                    this.attackDelayTimer = 0;
                    this.currentIndex++;

                }
                else
                {
                    rightArm.PrepareAttack();
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

    public virtual void TakeDamage(int damage)
    {
        if (health - damage <= 0) {
            health = 0;
            Destroy(this.gameObject);
        }
        else {
            health -= damage;
        }
    }
}

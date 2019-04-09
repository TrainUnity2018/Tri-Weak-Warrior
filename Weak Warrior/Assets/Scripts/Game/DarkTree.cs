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
    protected int currentHealth;
    protected int initHealth;
    public GameObject healthBar;
    public GameObject healthBarFrame;

    public Transform spawnLocation;
    public Transform attackLocation;
    public float spawnSpeed;
    public float dieSpeed;

    public List<ModelLevel_Boss> levels;
    protected ModelLevel_Boss currentLevel = null;
    protected int currentIndex = 0;
    protected float attackDelayTimer = 0;

    public ArmorGiver armorGiver;

    public enum MovementState
    {
        Spawning,
        Attack,
        Die,
    }
    public int currentMovementState;

    public bool pause;

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

    public virtual void Setup(Transform spawnLocation, Transform attackLocation)
    {
        this.spawnLocation = spawnLocation;
        this.attackLocation = attackLocation;
        this.currentIndex = 0;
        this.attackDelayTimer = 0;
        currentLevel = new ModelLevel_Boss();
        currentMovementState = (int)MovementState.Spawning;
        this.transform.position = new Vector3(spawnLocation.position.x, spawnLocation.position.y, 0);
        pause = false;
        initHealth = health;
        currentHealth = initHealth;
        healthBarFrame.SetActive(false);
    }

    public virtual void Spawning()
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
                healthBarFrame.SetActive(true);
                HealthBarScale();
            }
        }
    }
    public virtual void Attack()
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
        if (health - damage <= 0)
        {
            health = 0;
            currentHealth = health;
            HealthBarScale();
            currentMovementState = (int)DarkTree.MovementState.Die;
            Destroy(leftArm.gameObject);
            Destroy(rightArm.gameObject);
        }
        else
        {
            health -= damage;
            currentHealth = health;
            HealthBarScale();
        }
    }

    public virtual void OnDead()
    {
        if (currentMovementState == (int)MovementState.Die)
        {
            healthBarFrame.SetActive(false);
            if ((Mathf.Abs(transform.position.x - spawnLocation.position.x) > 0.01f) || (Mathf.Abs(transform.position.y - spawnLocation.position.y) > 0.01f))
            {
                Vector3 moveVector = (spawnLocation.position - transform.position).normalized;
                transform.position += moveVector * dieSpeed * Time.deltaTime;
            }
            else
            {
                ArmorGiver armorgiver = Instantiate(armorGiver, armorGiver.spawnLocation, Quaternion.identity) as ArmorGiver;
                EnemySpawnManager.Instance.spawnedArmorGiver.Add(armorgiver);
                Destroy(gameObject);              
            }
        }
    }

    public virtual void HealthBarScale()
    {
        float scaleRatio = (float)((float)currentHealth / (float)initHealth);
        Vector3 healthBarScale = healthBar.transform.localScale;
        healthBarScale.x = 0.1f * scaleRatio;
        healthBar.transform.localScale = healthBarScale;
    }

    public virtual void Pause()
    {
        pause = true;
    }

    public void UnPause()
    {
        pause = false;
    }
}

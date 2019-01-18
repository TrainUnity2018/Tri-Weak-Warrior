using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModelLevel
{
    public int level = -1;
    public float spawningDelay = 0.0f;
    public bool isSupper = false;
    public float moveSpeed = 1;
    public ModelLevel()
    {
        this.level = 0;
        this.spawningDelay = 0.3f;
    }

    public ModelLevel(int level, float spawningDelay)
    {
        this.level = level;
        this.spawningDelay = spawningDelay;
    }

    public ModelLevel(int level)
    {
        this.level = level;
        this.spawningDelay = 0.3f;
    }

}

[System.Serializable]
public class ModelEnemyNormal : ModelLevel
{
    public ModelEnemyNormal()
    { }

    public ModelEnemyNormal(int level)
    {
        this.level = level;
        this.moveSpeed = 1;
        this.spawningDelay = 0.3f;
    }

    public ModelEnemyNormal(int level, float spawningDelay)
    {
        this.level = level;
        this.moveSpeed = 1;
        this.spawningDelay = spawningDelay;
    }

    public ModelEnemyNormal(int level, float spawningDelay, int isSupperPercentage)
    {
        this.level = level;
        this.moveSpeed = 1;
        this.spawningDelay = spawningDelay;
        
        int rand = Random.Range(0, 100);

        if (rand <= isSupperPercentage)
        {
            this.isSupper = true;
            this.moveSpeed = 3;
        }
    }
}

public class EnemySpawnManager : MonoSingleton<EnemySpawnManager>
{

    public Transform spawnerLocationLeft;
    public Transform spawnerLocationRight;
    protected bool spawningLocation;
    public List<GoblinSwordman> enemyPrefabs;
    public List<ModelLevel> levels;
    private ModelLevel currentLevel = null;
    private int currentIndex = 0;
    private float spawningDelayTimer = 0;

    // Use this for initialization
    void Start()
    {
        this.Setup();
        this.SpawnEnemy();
    }

    private void Setup()
    {
        // this.levels = new List<ModelLevel>();
        // for (int i = 0; i < 50; i++)
        // {
        //     ModelEnemyNormal normal = new ModelEnemyNormal(Random.Range(0, 4));
        //     this.levels.Add(normal);
        // }
        this.currentIndex = 0;
        this.spawningDelayTimer = 0;

        this.levels = new List<ModelLevel>() {
            new ModelEnemyNormal(0, 1), new ModelEnemyNormal(0, 0.8f), new ModelEnemyNormal(0, 3), new ModelEnemyNormal(0, 1), new ModelEnemyNormal(0, 0.8f), new ModelEnemyNormal(0, 4),
            new ModelEnemyNormal(0, 1), new ModelEnemyNormal(0, 0.8f), new ModelEnemyNormal(0, 3), new ModelEnemyNormal(0, 1), new ModelEnemyNormal(0, 0.8f), new ModelEnemyNormal(0, 4, 70),
            new ModelEnemyNormal(0, 0.8f), new ModelEnemyNormal(0, 1), new ModelEnemyNormal(0, 3), new ModelEnemyNormal(0, 1, 70), new ModelEnemyNormal(0, 0.8f), new ModelEnemyNormal(0, 3),
            new ModelEnemyNormal(1, 1), new ModelEnemyNormal(1, 0.5f), new ModelEnemyNormal(1, 3), new ModelEnemyNormal(1, 1), new ModelEnemyNormal(1, 0.5f), new ModelEnemyNormal(1, 4),
            new ModelEnemyNormal(1, 1), new ModelEnemyNormal(1, 0.2f), new ModelEnemyNormal(1, 3), new ModelEnemyNormal(1, 0.5f), new ModelEnemyNormal(1, 0.5f), new ModelEnemyNormal(1, 3),
            new ModelEnemyNormal(2, 1), new ModelEnemyNormal(2, 0.8f), new ModelEnemyNormal(2, 0.8f), new ModelEnemyNormal(2, 3), new ModelEnemyNormal(2, 1), new ModelEnemyNormal(2, 0.8f),
            new ModelEnemyNormal(2, 3f), new ModelEnemyNormal(2, 1), new ModelEnemyNormal(2, 0.8f), new ModelEnemyNormal(2, 0.8f), new ModelEnemyNormal(2, 1.2f), new ModelEnemyNormal(2, 3),
            new ModelEnemyNormal(3, 1), new ModelEnemyNormal(3, 1), new ModelEnemyNormal(3, 3), new ModelEnemyNormal(3, 1), new ModelEnemyNormal(3, 0.5f), new ModelEnemyNormal(3, 0.5f),
            new ModelEnemyNormal(3, 1), new ModelEnemyNormal(3, 5),
         };
    }

    // Update is called once per frame
    void Update()
    {
        if (this.currentLevel != null)
        {
            this.spawningDelayTimer += Time.deltaTime;
            if (this.spawningDelayTimer >= this.currentLevel.spawningDelay)
            {
                this.SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        if (this.currentIndex < this.levels.Count)
        {
            this.currentLevel = this.levels[this.currentIndex];
            if (this.currentLevel != null)
            {
                int location = Random.Range(0, 2);
                if (location == 1)
                {
                    GoblinSwordman enemy = Instantiate(this.enemyPrefabs[this.currentLevel.level], this.transform) as GoblinSwordman;

                    if (enemy != null)
                    {
                        enemy.transform.position = new Vector3(this.spawnerLocationLeft.position.x, this.spawnerLocationLeft.position.y, 0);
                        enemy.Setup(true, this.currentLevel);
                        this.spawningDelayTimer = 0;
                        this.currentIndex++;
                    }
                }
                else
                {
                    GoblinSwordman enemy = Instantiate(this.enemyPrefabs[this.currentLevel.level], this.transform) as GoblinSwordman;

                    if (enemy != null)
                    {
                        enemy.transform.position = new Vector3(this.spawnerLocationRight.position.x, this.spawnerLocationRight.position.y, 0);
                        enemy.Setup(false, this.currentLevel);
                        this.spawningDelayTimer = 0;
                        this.currentIndex++;
                    }
                }

            }
        }
        else
        {
            this.Setup();
        }

    }
}

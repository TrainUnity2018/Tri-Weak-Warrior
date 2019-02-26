using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModelLevel
{
    public int enemyID = -1;
    public float spawningDelay = 0.0f;
    public bool isSupper = false;
    public float moveSpeed = 1;
    public ModelLevel()
    {
        this.enemyID = 0;
        this.spawningDelay = 0.3f;
    }

    public ModelLevel(int enemyID, float spawningDelay)
    {
        this.enemyID = enemyID;
        this.spawningDelay = spawningDelay;
    }

    public ModelLevel(int enemyID)
    {
        this.enemyID = enemyID;
        this.spawningDelay = 0.3f;
    }

}

[System.Serializable]
public class ModelEnemyNormal : ModelLevel
{
    public ModelEnemyNormal()
    { }

    public ModelEnemyNormal(int enemyID)
    {
        this.enemyID = enemyID;
        this.moveSpeed = 1;
        this.spawningDelay = 0.3f;
    }

    public ModelEnemyNormal(int enemyID, float spawningDelay)
    {
        this.enemyID = enemyID;
        this.moveSpeed = 1;
        this.spawningDelay = spawningDelay;
    }

    public ModelEnemyNormal(int enemyID, float spawningDelay, int isSuperPercentage)
    {
        this.enemyID = enemyID;
        this.moveSpeed = 1;
        this.spawningDelay = spawningDelay;

        int rand = Random.Range(0, 100);

        if (rand <= isSuperPercentage)
        {
            this.isSupper = true;
            this.moveSpeed = 3;
        }
    }

    public ModelEnemyNormal(int enemyID, float spawningDelay, int isSuperPercentage, float superMoveSpeed)
    {
        this.enemyID = enemyID;
        this.moveSpeed = 1;
        this.spawningDelay = spawningDelay;

        int rand = Random.Range(0, 100);

        if (rand <= isSuperPercentage)
        {
            this.isSupper = true;
            this.moveSpeed = superMoveSpeed;
        }
    }
}

public class EnemySpawnManager : MonoSingleton<EnemySpawnManager>
{

    public Transform spawnerLocationLeft;
    public Transform spawnerLocationRight;
    protected bool spawningLocation;
    public Transform bossSpawnerLocation;
    public Transform bossAttackLocation;
    public List<GoblinSwordman> enemyPrefabs;
    public List<DarkTree> bossPrefabs;
    public List<ModelLevel> levels;
    public List<GoblinSwordman> spawnedEnemies;
    public List<DarkTree> spawnedBosses;
    public int enemyLevelID;
    private ModelLevel currentLevel = null;
    private int currentIndex = 0;
    private float spawningDelayTimer = 0;
    private bool pause;

    // Use this for initialization
    void Start()
    {
        this.Setup();
        this.SpawnEnemy();
    }

    public void Setup()
    {
        // this.levels = new List<ModelLevel>();
        // for (int i = 0; i < 50; i++)
        // {
        //     ModelEnemyNormal normal = new ModelEnemyNormal(Random.Range(0, 4));
        //     this.levels.Add(normal);
        // }
        this.currentIndex = 0;
        this.spawningDelayTimer = 0;
        pause = false;
        enemyLevelID = 0;
        this.levels = new List<ModelLevel>() {
            new ModelEnemyNormal(0, 1), new ModelEnemyNormal(0, 0.8f), new ModelEnemyNormal(0, 3), new ModelEnemyNormal(0, 1), new ModelEnemyNormal(0, 0.8f), new ModelEnemyNormal(0, 4),
            new ModelEnemyNormal(0, 1), new ModelEnemyNormal(0, 0.8f), new ModelEnemyNormal(0, 3), new ModelEnemyNormal(0, 1), new ModelEnemyNormal(0, 0.8f), new ModelEnemyNormal(0, 4, 70, 3),
            new ModelEnemyNormal(0, 0.8f), new ModelEnemyNormal(0, 1), new ModelEnemyNormal(0, 3), new ModelEnemyNormal(0, 1, 70, 3), new ModelEnemyNormal(0, 0.8f), new ModelEnemyNormal(0, 3),
            new ModelEnemyNormal(1, 1), new ModelEnemyNormal(1, 0.5f), new ModelEnemyNormal(1, 3), new ModelEnemyNormal(1, 1), new ModelEnemyNormal(1, 0.5f), new ModelEnemyNormal(1, 4),
            new ModelEnemyNormal(1, 1), new ModelEnemyNormal(1, 0.2f), new ModelEnemyNormal(1, 3), new ModelEnemyNormal(1, 0.5f), new ModelEnemyNormal(1, 0.5f), new ModelEnemyNormal(1, 3),
            new ModelEnemyNormal(2, 1), new ModelEnemyNormal(2, 0.8f), new ModelEnemyNormal(2, 0.8f), new ModelEnemyNormal(2, 3), new ModelEnemyNormal(2, 1), new ModelEnemyNormal(2, 0.8f),
            new ModelEnemyNormal(2, 3f), new ModelEnemyNormal(2, 1), new ModelEnemyNormal(2, 0.8f), new ModelEnemyNormal(2, 0.8f), new ModelEnemyNormal(2, 1.2f), new ModelEnemyNormal(2, 3),
            new ModelEnemyNormal(3, 1), new ModelEnemyNormal(3, 1), new ModelEnemyNormal(3, 3), new ModelEnemyNormal(3, 1), new ModelEnemyNormal(3, 0.5f), new ModelEnemyNormal(3, 0.5f),
            new ModelEnemyNormal(3, 1), new ModelEnemyNormal(3, 8), new ModelEnemyNormal(-1, 5),
         };
        // this.levels = new List<ModelLevel>() {
        //      new ModelEnemyNormal(-1, 5),
        //  };
    }

    // Update is called once per frame
    void Update()
    {
        if (this.currentLevel != null && !pause)
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
                if (currentLevel.enemyID == -1)
                {
                    DarkTree boss = Instantiate(this.bossPrefabs[0], this.transform) as DarkTree;
                    boss.transform.position = new Vector3(this.bossSpawnerLocation.position.x, this.bossSpawnerLocation.position.y, 0);
                    boss.Setup(bossSpawnerLocation, bossAttackLocation);
                    this.spawningDelayTimer = 0;
                    this.currentIndex++;
                    spawnedBosses.Add(boss);
                }
                else
                {
                    int location = Random.Range(0, 2);
                    if (location == 1)
                    {
                        GoblinSwordman enemy = Instantiate(this.enemyPrefabs[this.currentLevel.enemyID], this.transform) as GoblinSwordman;

                        if (enemy != null)
                        {
                            enemy.transform.position = new Vector3(this.spawnerLocationLeft.position.x, this.spawnerLocationLeft.position.y, 0);
                            enemy.Setup(true, this.currentLevel);
                            this.spawningDelayTimer = 0;
                            this.currentIndex++;
                            spawnedEnemies.Add(enemy);
                            if (enemyLevelID < this.currentLevel.enemyID)
                            {
                                enemyLevelID = this.currentLevel.enemyID;
                            }
                        }
                    }
                    else
                    {
                        GoblinSwordman enemy = Instantiate(this.enemyPrefabs[this.currentLevel.enemyID], this.transform) as GoblinSwordman;

                        if (enemy != null)
                        {
                            enemy.transform.position = new Vector3(this.spawnerLocationRight.position.x, this.spawnerLocationRight.position.y, 0);
                            enemy.Setup(false, this.currentLevel);
                            this.spawningDelayTimer = 0;
                            this.currentIndex++;
                            spawnedEnemies.Add(enemy);
                            if (enemyLevelID < this.currentLevel.enemyID)
                            {
                                enemyLevelID = this.currentLevel.enemyID;
                            }
                        }
                    }
                }


            }
        }
        else
        {
            //this.Setup();
        }

    }

    public void Pause()
    {
        pause = true;
        if (spawnedEnemies.Count > 0)
            for (int i = 0; i < spawnedEnemies.Count; i++)
                spawnedEnemies[i].Pause();
        if (spawnedBosses.Count > 0)
            for (int i = 0; i < spawnedBosses.Count; i++)
                spawnedBosses[i].Pause();
    }

    public void UnPause()
    {
        pause = false;
        if (spawnedEnemies.Count > 0)
            for (int i = 0; i < spawnedEnemies.Count; i++)
                spawnedEnemies[i].UnPause();
        if (spawnedBosses.Count > 0)
            for (int i = 0; i < spawnedBosses.Count; i++)
                spawnedBosses[i].Pause();
    }
}

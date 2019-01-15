using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ModelLevel
{
    public int level = -1;
    public float time = 0.0f;
    public bool isSupper = false;
    public float speed = 0.0f;
    public int getLevel()
    {
        return this.level;
    }
    public float getTime()
    {
        return this.time;
    }
    public ModelLevel()
    { }
    public ModelLevel(int level, float time)
    {
        this.level = level;
        this.time = time;
    }
    public ModelLevel(int level)
    {
        this.level = level;
        this.time = 0.3f;// Random.Range(0.2f, 0.5f);
        this.speed = 100f;// Random.Range(100.0f, 200.0f);
    }

}
[System.Serializable]
public class ModelEnemyNormal : ModelLevel
{
    public ModelEnemyNormal()
    { }
    public ModelEnemyNormal(int lv)
    {
        this.level = lv;
        this.speed = 1;// Random.Range(0.5f, 2.0f);
        this.time = Random.Range(1.0f, 3.0f);
        int rand = Random.Range(0, 100);
        
        if(rand < 10)
        {
            this.isSupper = true;
            this.speed = 2;

        }

    }
}



public class EnemySpawnManager : MonoSingleton<EnemySpawnManager> {

    public Transform spawnerLeft;
    public Transform spawnerRight;
    protected bool spawningLocation;
    public List<GoblinSwordman> enemyPrefabs;
    public List<ModelLevel> levels;// = new List<ModelLevel>() { new ModelLevel(0), new ModelLevel(0), new ModelLevel(1) };
    private ModelLevel currentLevel = null;
    private int indexCurrent = 0;
    private float timeDelay = 0;
    // Use this for initialization
	void Start () {
        
        this.Setup();
    }
	private void Setup()
    {
        this.levels = new List<ModelLevel>();
        for (int i = 0; i < 10; i++)
        {
            ModelEnemyNormal normal = new ModelEnemyNormal(0);
            this.levels.Add(normal);
        }

        this.indexCurrent = 0;
        this.timeDelay = 0;
        this.SpawnEnemy();
    }
	// Update is called once per frame
	void Update () {
        if (this.currentLevel != null)
        {
            this.timeDelay += Time.deltaTime;
            if(this.timeDelay >= this.currentLevel.getTime())
            {
                this.SpawnEnemy();
            }
        }

    }

    private void SpawnEnemy()
    {
        if(this.indexCurrent < this.levels.Count)
        {
            this.currentLevel = this.levels[this.indexCurrent];
            if (this.currentLevel != null)
            {
                int location = Random.Range(-1, 2);
                if (location == 0)
                {
                    GoblinSwordman enemy = Instantiate(this.enemyPrefabs[this.currentLevel.getLevel()], this.transform) as GoblinSwordman;

                    if (enemy != null)
                    {
                        enemy.transform.position = new Vector3(this.spawnerLeft.position.x, this.spawnerLeft.position.y, 0);
                        enemy.Setup(true, this.currentLevel);
                        this.timeDelay = 0;
                        this.indexCurrent++;
                    }
                }
                else
                {
                    GoblinSwordman enemy = Instantiate(this.enemyPrefabs[this.currentLevel.getLevel()], this.transform) as GoblinSwordman;

                    if (enemy != null)
                    {
                        enemy.transform.position = new Vector3(this.spawnerRight.position.x, this.spawnerRight.position.y, 0);
                        enemy.Setup(false, this.currentLevel);
                        this.timeDelay = 0;
                        this.indexCurrent++;
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

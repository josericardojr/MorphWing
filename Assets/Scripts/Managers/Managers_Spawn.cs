using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers_Spawn : MonoBehaviour 
{
    bool deactivaded;
	Vector3 selectedPos;
	[SerializeField]
	List<Vector3> positionsList = new List<Vector3>();
	[SerializeField]
	List<GameObject> enemyObjects = new List<GameObject>();
    [SerializeField]
    GameObject itemPrefab;
	[SerializeField]
	private float maxSpawnOffsetX = 0, maxSpawnOffsetY = 0;

	private int noSpawned;

    #region GETS & SETS

    public bool Deactivated
    {
        get { return this.deactivaded; }
        set { this.deactivaded = value; }
    }

    public float MaxOffsetX
    {
        get { return this.maxSpawnOffsetX; }
        set { this.maxSpawnOffsetX = value; }
    }

    public float MaxOffsetY
    {
        get { return this.maxSpawnOffsetY; }
        set { this.maxSpawnOffsetY = value; }
    }

    #endregion

    void Start()
	{
		SpawnEnemies();
	}

    void Update()
    {
        if (this.deactivaded)
            Retry();
    }

    void Retry()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            SceneManager.LoadScene(0);
    }

	public void SpawnEnemies()
	{
        if(!this.deactivaded)
		    for(int i = 0; i < 4; i++)
		    {
			    this.noSpawned++;
			    SpawnRandomEnemy();
		    }
	}

	public void EnemyDecrease()
	{
		this.noSpawned--;
		if(this.noSpawned < 4)
        {
            SpawnRandomEnemy();
            this.noSpawned++;
            if (Random.Range(0, 6) == 0) 
                SpawnRandomItem();
        }
	}

	void SpawnRandomEnemy()
	{
		// Define Position
		this.selectedPos = this.positionsList[Random.Range(0, this.positionsList.Count)];
		SetOffsets();
		int enemyGot = Random.Range(0, this.enemyObjects.Count);
		GameObject spawnedEnemy = (GameObject)GameObject.Instantiate(this.enemyObjects[enemyGot], selectedPos, Quaternion.identity);
		spawnedEnemy.GetComponent<Characters_Enemies>().InitDir = new Vector2(-selectedPos.x, -selectedPos.y);
        spawnedEnemy.GetComponent<Characters_Enemies>().MaxOffsetX = this.maxSpawnOffsetX;
        spawnedEnemy.GetComponent<Characters_Enemies>().MaxOffsetY = this.maxSpawnOffsetY;
    }

    void SpawnRandomItem()
    {
        // Define Position
        GameObject spawnedItem = (GameObject)GameObject.Instantiate(this.itemPrefab, new Vector3(Random.Range((float)-this.maxSpawnOffsetX, (float)this.maxSpawnOffsetX), Random.Range((float)-this.maxSpawnOffsetY, (float)this.maxSpawnOffsetY), 1), Quaternion.identity);
    }

	void SetOffsets()
	{
		List<float> spawnOffsets = new List<float>();
		spawnOffsets.Add((float)Random.Range(-this.maxSpawnOffsetX, this.maxSpawnOffsetX));
		spawnOffsets.Add(this.maxSpawnOffsetX);
		int offsetGot = Random.Range(0, 2);
		this.selectedPos = new Vector3(this.selectedPos.x * spawnOffsets[offsetGot], this.selectedPos.y, this.selectedPos.z);
		spawnOffsets.RemoveAt(offsetGot);
		this.selectedPos = new Vector3(this.selectedPos.x, this.selectedPos.y * spawnOffsets[0], this.selectedPos.z);
		this.selectedPos = new Vector3(this.selectedPos.x, Mathf.Clamp(this.selectedPos.y, -this.maxSpawnOffsetY, this.maxSpawnOffsetY), this.selectedPos.z);
	}
}

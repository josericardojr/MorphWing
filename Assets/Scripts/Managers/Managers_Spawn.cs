using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers_Spawn : MonoBehaviour 
{
	Vector3 selectedPos;
	[SerializeField]
	List<Vector3> positionsList = new List<Vector3>();
	[SerializeField]
	List<GameObject> enemyObjects = new List<GameObject>();
	[SerializeField]
	private float maxSpawnOffsetX = 0, maxSpawnOffsetY = 0;

	private int noSpawned;

	void Start()
	{
		SpawnEnemies();
	}

	public void SpawnEnemies()
	{
		for(int i = 0; i < 4; i++)
		{
			this.noSpawned++;
			SpawnRandomEnemy();
		}
	}

	public void EnemyDecrease()
	{
		this.noSpawned--;
		if(this.noSpawned <= 0)
			SpawnEnemies();
	}

	void SpawnRandomEnemy()
	{
		// Define Position
		this.selectedPos = this.positionsList[Random.Range(0, this.positionsList.Count)];
		SetOffsets();
		int enemyGot = Random.Range(0, this.enemyObjects.Count);
		GameObject spawnedEnemy = (GameObject)GameObject.Instantiate(this.enemyObjects[enemyGot], selectedPos, Quaternion.identity);
		spawnedEnemy.GetComponent<Characters_Enemies>().InitDir = new Vector2(-selectedPos.x, -selectedPos.y);
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

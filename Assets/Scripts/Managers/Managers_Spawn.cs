using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers_Spawn : MonoBehaviour 
{
	public ItemController itemController;
	protected ExtractProvenance extractProvenance;
	bool deactivaded, canSpawn;
    [SerializeField]
    bool random;
    float spawnTime = 0.4f, currSpawnTime;
	Vector3 selectedPos;
	[SerializeField]
	List<GameObject> enemyObjects = new List<GameObject>();
	[SerializeField]
	List<Transform> straightPositions, boomerangPositions, roundPositions, chaserPositions, healthPositions, allPositions;
	[SerializeField]
	List<int> waveCreation;
	List<int> currPositions = new List<int>();
	int currWave = 0, currHealth = 0;
	[SerializeField]
	GameObject itemPrefab;
	[SerializeField]
	private float maxSpawnOffsetX = 0, maxSpawnOffsetY = 0;
    [SerializeField]
    List<int> enemySpawns;

	#region GETS & SETS

    public List<int> EnemySpawns
    {
        get { return this.enemySpawns; }
        set { this.enemySpawns = value; }
    }

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
		this.extractProvenance = this.GetComponent<ExtractProvenance>();
		SpawnEnemies();
	}

	public void SpawnEnemies()
	{
		if(!this.deactivaded)
			for(int i = 0; i < 4; i++)
			{
                SpawnNextEnemy();
			}
        Invoke("CanStartSpawning", 1);
	}

    void Update()
    {
        if (canSpawn && this.currSpawnTime == 0)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length < 4)
            {
                if(currSpawnTime <= 0)
                    this.currSpawnTime = this.spawnTime;
                //if (this.currWave == this.waveCreation.Count)
                //    SpawnRandomItem();
                SpawnNextEnemy();
            }
        }
        if (currSpawnTime > 0)
        {
            this.currSpawnTime -= Time.deltaTime;
        }
        if (this.currSpawnTime < 0)
            this.currSpawnTime = 0;
    }

    void CanStartSpawning()
    {
        this.canSpawn = true;
    }

	void SpawnNextEnemy()
	{
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (this.currPositions.Count == 0)
            for (int i = 0; i < 4; i++)
                this.currPositions.Add(0);
        SetOffsets();
        if (this.currWave == this.waveCreation.Count)
            this.currWave = 0;
        int enemyGot = this.waveCreation[this.currWave];
        if (this.random)
            enemyGot = Random.Range(0, 4);
        Vector2 spawnPos = new Vector2(0, 0);
        Transform spawnPoint = null;
        if (!this.random)
        {
            switch (enemyGot)
            {
                case 0:
                    if (this.currPositions[0] == this.straightPositions.Count)
                        this.currPositions[0] = 0;
                    spawnPos = this.straightPositions[this.currPositions[0]].transform.position;
                    spawnPoint = this.straightPositions[this.currPositions[0]];
                    this.currPositions[0] += 1;
                    break;
                case 1:
                    if (this.currPositions[1] == this.chaserPositions.Count)
                        this.currPositions[1] = 0;
                    spawnPos = this.chaserPositions[this.currPositions[1]].transform.position;
                    spawnPoint = this.chaserPositions[this.currPositions[1]];
                    this.currPositions[1] += 1;
                    break;
                case 2:
                    if (this.currPositions[2] == this.boomerangPositions.Count)
                        this.currPositions[2] = 0;
                    spawnPos = this.boomerangPositions[this.currPositions[2]].transform.position;
                    spawnPoint = this.boomerangPositions[this.currPositions[2]];
                    this.currPositions[2] += 1;
                    break;
                case 3:
                    if (this.currPositions[3] == this.roundPositions.Count)
                        this.currPositions[3] = 0;
                    spawnPos = this.roundPositions[this.currPositions[3]].transform.position;
                    spawnPoint = this.roundPositions[this.currPositions[3]];
                    this.currPositions[3] += 1;
                    break;
            }
        }
        else
        {
            spawnPoint = this.allPositions[Random.Range(0, 14)];
            spawnPos = spawnPoint.transform.position;
        }
        GameObject spawnedEnemy = (GameObject)GameObject.Instantiate(this.enemyObjects[enemyGot], new Vector3(spawnPos.x, spawnPos.y, 1), Quaternion.identity);
        spawnedEnemy.name = spawnedEnemy.name.Remove(spawnedEnemy.name.Length - 7);
        this.currWave++;
        spawnedEnemy.GetComponent<Characters_Enemies>().InitDir = new Vector2(spawnPoint.GetComponent<Objects_SpawnPoint>().InitDir.x, spawnPoint.GetComponent<Objects_SpawnPoint>().InitDir.y);
        spawnedEnemy.GetComponent<Characters_Enemies>().MaxOffsetX = this.maxSpawnOffsetX;
        spawnedEnemy.GetComponent<Characters_Enemies>().MaxOffsetY = this.maxSpawnOffsetY;
	}

	void SpawnRandomItem()
	{
		// Define Position
		if(this.currHealth == this.healthPositions.Count)
			this.currHealth = 0;
        GameObject spawnedItem;
        if(!this.random)
            spawnedItem = (GameObject)GameObject.Instantiate(this.itemPrefab, new Vector3(this.healthPositions[this.currHealth].position.x, this.healthPositions[this.currHealth].position.y, 1), Quaternion.identity);
        else
            spawnedItem = (GameObject)GameObject.Instantiate(this.itemPrefab, new Vector3(this.healthPositions[Random.Range(0, 4)].position.x, this.healthPositions[Random.Range(0, 4)].position.y, 1), Quaternion.identity);
		spawnedItem.GetComponent<Item> ().effect = Object_Efeitos.Effects.HEALTH;
		spawnedItem.GetComponent<Item> ().itemController = this.itemController;
		itemController.Increase (Object_Efeitos.Effects.HEALTH.ToString ());
		Prov_Spawn_Health_Item(spawnedItem.GetComponent<Collider2D>().GetInstanceID().ToString());
		this.currHealth++;
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

	protected void Prov_Spawn_Health_Item(string id)
	{
		int DU = itemController.GetCout(Object_Efeitos.Effects.DAMAGE_UP.ToString());
		int DD = itemController.GetCout(Object_Efeitos.Effects.DAMAGE_DOWN.ToString());
		int SU = itemController.GetCout(Object_Efeitos.Effects.SPEED_UP.ToString());
		int SD = itemController.GetCout(Object_Efeitos.Effects.SPEED_DOWN.ToString());
		int IC = itemController.GetCout(Object_Efeitos.Effects.INVERT_CONTROL.ToString());
		int HT = itemController.GetCout(Object_Efeitos.Effects.HEALTH.ToString());

		this.extractProvenance.AddAttribute ("Total_Items", "DU: " + DU + " DD: " + DD +
			" SU: " + SU + " SD: " + SD + " IC: " + IC + " HT: " + HT);


		this.extractProvenance.NewActivityVertex("Item Spawned");
		this.extractProvenance.AddAttribute("Heal", 2.ToString());
		this.extractProvenance.AddAttribute("infID", id);
		this.extractProvenance.NewEntityVertex("Health Item");
		this.extractProvenance.GenerateInfluenceC("Heal", id, "Health (Player)", "2", 1);
	}
}

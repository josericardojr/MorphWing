using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Characters_Global : MonoBehaviour
{
	List<GameObject> prefabList = new List<GameObject>();

	protected ExtractProvenance extractProvenance;
	protected Animator animator;
	new protected Rigidbody2D rigidbody;

	[SerializeField]
	protected float stat_speed;

	[SerializeField]
	protected int stat_hp; 
	protected int temp_currHp, dirX, dirY;

	[SerializeField]
	protected string provIndentifier, objType;
	protected string lastHitBy = "";

	[SerializeField]
	protected List<string> prefabNames = new List<string>();

	protected void Start()
	{
		this.temp_currHp = this.stat_hp;
		GameObject provenanceObj = GameObject.FindGameObjectWithTag("Provenance");
		this.animator = this.GetComponent<Animator>();
		this.extractProvenance = this.GetComponent<ExtractProvenance>();
		this.extractProvenance.influenceContainer = provenanceObj.GetComponent<InfluenceController>();
		this.extractProvenance.provenance = provenanceObj.GetComponent<ProvenanceController>();
		this.rigidbody = this.GetComponent<Rigidbody2D>();
		ProjectilesLoad();
	}

	void ProjectilesLoad()
	{
		for(int i = 0; i < this.prefabNames.Count; i++)
			this.prefabList.Add((GameObject)Resources.Load("Prefabs/Projectiles/Projectiles_" + this.prefabNames[i]));
	}

	protected void MovementCall()
	{
		this.rigidbody.velocity = new Vector2 (this.dirX * this.stat_speed, this.dirY * this.stat_speed);
	}

	public virtual void GetDamaged(float instanceID, string objLabel, float damage)
	{
		this.lastHitBy = objLabel;
		this.Prov_TakeDamage(instanceID, damage);
		this.animator.SetTrigger("Flash");
		this.temp_currHp -= (int)damage;
		CheckIfAlive();
	}

	protected virtual void CheckIfAlive()
	{
		if(this.temp_currHp <= 0)
			Destroy(this.gameObject);
	}

	protected virtual void ShootProjectile(int projIndex, int passDirX, int passDirY)
	{

		GameObject projectile = GameObject.Instantiate(this.prefabList[projIndex], this.transform.position, Quaternion.identity);
		projectile.GetComponent<Projectiles_Global>().StatsReceiver(this.gameObject, 3, passDirX, passDirY, projectile.GetComponent<Collider2D>().GetInstanceID(), this.provIndentifier);
		if (this.provIndentifier.Equals("Player"))
			Prov_PlayerShoot(projectile.GetComponent<Projectiles_Global>().Damage, projectile.GetComponent<Collider2D>().GetInstanceID().ToString());
		else
			Prov_EnemyShoot(projectile.GetComponent<Projectiles_Global>().Damage, projectile.GetComponent<Collider2D>().GetInstanceID().ToString());
	}

	#region GETS e SETS
	public float GetSpeed()
	{
		return this.stat_speed;
	}

	public void SetSpeed(float value)
	{
		this.stat_speed = value;
	}

	public List<GameObject> PrefabList
	{
		get { return this.prefabList; }
		set { this.prefabList = value; }
	}


	#endregion

	#region Provenance

	protected void Prov_Agent()
	{
		this.Prov_GetAttributes ();
		this.extractProvenance.NewAgentVertex (this.provIndentifier);
	}

	protected void Prov_HP()
	{
		Prov_GetAttributes();
		this.extractProvenance.NewActivityVertex("HP(" + this.provIndentifier + ")");
		this.extractProvenance.HasInfluence(this.provIndentifier);
	}

	protected virtual void Prov_GetAttributes()
	{
		this.extractProvenance.AddAttribute ("HP", this.temp_currHp.ToString());
		this.extractProvenance.AddAttribute("Speed", this.stat_speed.ToString());
		this.extractProvenance.AddAttribute("Last", this.lastHitBy);
		this.extractProvenance.AddAttribute("Enemies", "S: " + GetEnemyNo("Straight") + " C: " + GetEnemyNo("Chaser") +
			" I: " + GetEnemyNo("Irregular") + " R: " + GetEnemyNo("Round"));
	}

	int GetEnemyNo(string enemyProvID)
	{
		int enemyNo = 0;
		foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
		{
			if(enemy.GetComponent<Characters_Global>().provIndentifier.Equals(enemyProvID))
				enemyNo++;
		}
		return enemyNo;
	}

	public void Prov_TakeDamage(float instanceID, float damageAmount)
	{
		//Prov_GetAttributes();
		string infID = instanceID.ToString();
		this.Prov_TakeDamage(infID);
		//this.extractProvenance.GenerateInfluenceCE("Damage", this.GetInstanceID().ToString(), "Health (" + this.name + ")", (-damageAmount).ToString(), 1, Time.time + 5); 
	}

	protected void Prov_Heal(string infID)
	{
		Prov_GetAttributes();
		this.extractProvenance.AddAttribute("infID", infID);
		this.extractProvenance.NewActivityVertex("Heal");
		this.extractProvenance.HasInfluence_ID(infID);
	}

	/*
    public string Prov_Attack(float damageAmount)
    {
        Prov_GetAttributes();
        this.extractProvenance.NewActivityVertex("Attacking", this.gameObject);
        this.extractProvenance.HasInfluence("Enemy");
        this.extractProvenance.GenerateInfluenceCE("PlayerDamage", this.GetInstanceID().ToString(), "Health (Player)", (-damageAmount).ToString(), 1, Time.time + 5);
        return this.GetInstanceID().ToString();
    }*/

	public void Prov_TakeDamage(string infID)
	{

		this.Prov_GetAttributes();
		this.extractProvenance.NewActivityVertex("Being Hit(" + this.objType + ")");
		// Check Influence
		//this.extractProvenance.HasInfluence(this.lastHitBy);
		this.extractProvenance.HasInfluence_ID(infID);
		if(this.provIndentifier.Equals("Player"))
			this.extractProvenance.GenerateInfluenceE("Invencibilidade", this.GetInstanceID().ToString(), 
				"Invulnerability", "", Time.time + 1);

	}
	/*
	public void Prov_TakeDamage()
	{
		this.Prov_GetAttributes();
		this.extractProvenance.NewActivityVertex("Being Hit");
		// Check Influence
		this.extractProvenance.HasInfluence(this.provIndentifier + "Damage");
	}
    */

	public void Prov_PowerUp(string type, string infID)
	{
		Prov_GetAttributes();
		this.extractProvenance.AddAttribute("InfID", infID);
		this.extractProvenance.NewActivityVertex(type);
		this.extractProvenance.HasInfluence_ID(infID);
		//this.extractProvenance.provenance.Save("info");
	}

	public string Prov_EnemyAttack(int damageAmount)
	{
		this.Prov_GetAttributes();
		this.extractProvenance.NewActivityVertex("Colliding", this.gameObject);
		this.extractProvenance.HasInfluence("Enemy");
		this.extractProvenance.GenerateInfluenceCE("PlayerDamage", this.GetInstanceID().ToString(), "Health (Player)", (-damageAmount).ToString(), 1, Time.time + 0.5f);
		return this.GetInstanceID().ToString();
	}

	public string Prov_PlayerShoot(float damageAmount, string infID)
	{
		this.Prov_GetAttributes();
		this.extractProvenance.NewActivityVertex("Firing", this.gameObject);
		this.extractProvenance.HasInfluence("Player");
		this.extractProvenance.GenerateInfluenceCE("EnemyDamage", infID, "Health (Enemy)", (-damageAmount).ToString(), 1, Time.time + 0.5f);
		return this.GetInstanceID().ToString();
	}

	public string Prov_EnemyShoot(float damageAmount, string infID)
	{
		this.Prov_GetAttributes();
		this.extractProvenance.NewActivityVertex("Firing", this.gameObject);
		this.extractProvenance.HasInfluence("Enemy");
		this.extractProvenance.GenerateInfluenceCE("PlayerDamage", infID, "Health (Player)", (-damageAmount).ToString(), 1, Time.time + 0.5f);
		return this.GetInstanceID().ToString();
	}

	#endregion

}

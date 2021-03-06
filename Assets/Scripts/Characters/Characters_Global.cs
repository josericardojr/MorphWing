﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PinGU;

public abstract class Characters_Global : MonoBehaviour
{
    protected BalanceApplier balanceApplier;
	protected List<GameObject> prefabList = new List<GameObject>();
	protected ScoreManager scoreManager;
	protected ExtractProvenance extractProvenance;
	protected Animator animator;
	new protected Rigidbody2D rigidbody;

    protected bool initialInvic, dead;

	[SerializeField]
	protected float stat_speed;
    public float speedMultiplier = 1;

	[SerializeField]
	protected float stat_hp;
    protected float temp_currHp;
    protected int dirX, dirY;

	[SerializeField]
	protected string provIndentifier, objType;
	protected string lastHitBy = "";

	[SerializeField]
	protected List<string> prefabNames = new List<string>();

  private AccessPython acessPython;

  public float Temp_CurrHp
  {
      get { return this.temp_currHp; }
      set { this.temp_currHp = value; }
  }

  public float Stat_HP
  {
      get { return this.stat_hp; }
      set { this.stat_hp = value; }
  }

	protected void Start()
	{
        this.provIndentifier = provIndentifier + "_" + this.GetInstanceID().ToString();
        this.balanceApplier = GameObject.Find("Balance").GetComponent<BalanceApplier>();
		this.temp_currHp = this.stat_hp;
		GameObject provenanceObj = GameObject.Find("Provenance");
		this.animator = this.GetComponent<Animator>();
		this.extractProvenance = this.GetComponent<ExtractProvenance>();
		this.scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
		//this.extractProvenance.influenceContainer = provenanceObj.GetComponent<InfluenceController>();
		//this.extractProvenance.provenance = provenanceObj.GetComponent<ProvenanceController>();
		this.rigidbody = this.GetComponent<Rigidbody2D>();
		ProjectilesLoad();
    acessPython = FindObjectOfType<AccessPython>();
	}

	void ProjectilesLoad()
	{
		for(int i = 0; i < this.prefabNames.Count; i++)
			this.prefabList.Add((GameObject)Resources.Load("Prefabs/Projectiles/Projectiles_" + this.prefabNames[i]));
	}

	protected void MovementCall()
	{
        this.rigidbody.velocity = new Vector2(this.dirX * this.stat_speed * speedMultiplier, this.dirY * this.stat_speed * speedMultiplier);
	}

	public virtual void GetDamaged(float instanceID, string objLabel, float damage)
	{
        if (!this.initialInvic)
        {
            this.lastHitBy = objLabel;
            this.Prov_TakeDamage(instanceID, damage);
            this.animator.SetTrigger("Flash");
            this.temp_currHp -= damage;
            CheckIfAlive(instanceID);
        }
	}

    public virtual void CheckIfAlive(float instanceID)
	{
        if (this.temp_currHp <= 0)
        {
            this.Prov_GetDestroyed(instanceID);
            Invoke("Destroy", 0.1f);
        }
	}

    void Destroy()
    {
        Destroy(this.gameObject);
    }

    protected virtual void ShootProjectile(int projIndex, int passDirX, int passDirY)
	{
		GameObject projectile = GameObject.Instantiate(this.prefabList[projIndex], this.transform.position, Quaternion.identity);
		projectile.GetComponent<Projectiles_Global>().StatsReceiver(this.gameObject, 3, passDirX, passDirY, this.GetInstanceID(), this.provIndentifier);
		if (this.provIndentifier.Equals("Player"))
			Prov_PlayerShoot(projectile.GetComponent<Projectiles_Global>().Damage, this.GetInstanceID().ToString());
		//else
			//Prov_EnemyShoot(projectile.GetComponent<Projectiles_Global>().Damage, projectile.GetComponent<Collider2D>().GetInstanceID().ToString());
	}

	#region GETS e SETS

	public float Stat_Speed
	{
        get{return this.stat_speed;}
        set{this.stat_speed = value;}
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
		this.extractProvenance.AddAttribute("LocalTime", this.scoreManager.ElapsedTime.ToString());
		this.extractProvenance.AddAttribute ("HP", this.temp_currHp.ToString());
		this.extractProvenance.AddAttribute("Speed", this.stat_speed.ToString());
		this.extractProvenance.AddAttribute("Last", this.lastHitBy);
        this.extractProvenance.AddAttribute("LastType", this.lastHitBy.Split('_')[0]);
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
		Prov_GetAttributes();
		string infID = instanceID.ToString();
        this.Prov_TakeDamage(infID);
        // Check Influence
		this.extractProvenance.GenerateInfluenceCE("Damage", this.GetInstanceID().ToString(), "Health (" + this.name + ")", (-damageAmount).ToString(), 1, Time.time + 5); 
	}

    public void Prov_TakeDamage(string infID)
    {
        AcessPython.Instance.AddContVertx();
        this.Prov_GetAttributes();
        this.extractProvenance.AddAttribute("InstanceID", this.provIndentifier);
        this.extractProvenance.NewActivityVertex("Being Hit(" + this.objType + ")");
        // Check Influence
        this.extractProvenance.HasInfluence(this.lastHitBy);
        this.extractProvenance.HasInfluence_ID(infID);
        if (this.provIndentifier.Equals("Player"))
            this.extractProvenance.GenerateInfluenceE("Invencibilidade", this.GetInstanceID().ToString(),
                "Invulnerability", "", Time.time + 1);

    }

    public void Prov_GetDestroyed(float instanceID)
    {
        Prov_GetAttributes();
        string infID = instanceID.ToString();
        this.Prov_GetDestroyed(infID);
        //
        this.extractProvenance.GenerateInfluenceCE("Destroyed", this.GetInstanceID().ToString(), "Health (" + this.name + ")", "", 1, Time.time + 5);
    }

    public void Prov_GetDestroyed(string infID)
    {
        AcessPython.Instance.AddContVertx();
        this.Prov_GetAttributes();
        this.extractProvenance.NewActivityVertex("Destroyed(" + this.objType + ")");
        // Check Influence
        this.extractProvenance.HasInfluence(this.lastHitBy);
        this.extractProvenance.HasInfluence_ID(infID);
    }

    protected void Prov_Heal(string infID)
	{
		Prov_GetAttributes();
		this.extractProvenance.AddAttribute("infID", infID);
		this.extractProvenance.NewActivityVertex("Heal");
		this.extractProvenance.HasInfluence_ID(infID);
        AcessPython.Instance.AddContVertx();
	}

    protected void Prov_UsingAttack(string infID)
    {
        Prov_GetAttributes();
        this.extractProvenance.AddAttribute("infID", infID);
        this.extractProvenance.NewActivityVertex("Attacking (" + this.objType + ")");
        this.extractProvenance.HasInfluence_ID(infID);
        AcessPython.Instance.AddContVertx();
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
        AcessPython.Instance.AddContVertx();
		Prov_GetAttributes();
		this.extractProvenance.AddAttribute("InfID", infID);
		this.extractProvenance.NewActivityVertex(type);
		this.extractProvenance.HasInfluence_ID(infID);
		//this.extractProvenance.provenance.Save("info");
	}

	public string Prov_EnemyAttack(int damageAmount)
    {
        AcessPython.Instance.AddContVertx();
		this.Prov_GetAttributes();
		this.extractProvenance.NewActivityVertex("Colliding", this.gameObject);
		this.extractProvenance.HasInfluence("Enemy");
		this.extractProvenance.GenerateInfluenceCE("PlayerDamage", this.GetInstanceID().ToString(), "Health (Player)", (-damageAmount).ToString(), 1, Time.time + 0.5f);
		return this.GetInstanceID().ToString();
	}

	public string Prov_PlayerShoot(float damageAmount, string infID)
    {
        AcessPython.Instance.AddContVertx();
		this.Prov_GetAttributes();
		this.extractProvenance.NewActivityVertex("Firing", this.gameObject);
		this.extractProvenance.HasInfluence("Player");
		this.extractProvenance.GenerateInfluenceCE("EnemyDamage", infID, "Health (Enemy)", (-damageAmount).ToString(), 1, Time.time + 0.5f);
		return this.GetInstanceID().ToString();
	}

	public string Prov_EnemyShoot(float damageAmount, string infID)
    {
        AcessPython.Instance.AddContVertx();
		this.Prov_GetAttributes();
		this.extractProvenance.NewActivityVertex("Firing", this.gameObject);
		this.extractProvenance.HasInfluence("Enemy");
		this.extractProvenance.GenerateInfluenceCE("PlayerDamage", infID, "Health (Player)", (-damageAmount).ToString(), 1, Time.time + 0.5f);
		return this.GetInstanceID().ToString();
	}

	#endregion

}

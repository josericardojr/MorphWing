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
	private float stat_speed;

	[SerializeField]
	protected int stat_hp; 
	protected int temp_currHp, dirX, dirY;

	[SerializeField]
	protected string provIndentifier;

    [SerializeField]
    protected List<string> prefabNames = new List<string>();

    
	GameObject projectilePrefab;

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

	public virtual void GetDamaged(GameObject attacker, int damage)
	{
		this.animator.SetTrigger("Flash");
		this.temp_currHp -= damage;
		CheckIfAlive();
		this.Prov_TakeDamage(attacker, damage);
	}

	protected void CheckIfAlive()
	{
		if(this.temp_currHp <= 0)
			Destroy(this.gameObject);
	}

	protected void ShootProjectile(int projIndex)
	{
		GameObject projectile = GameObject.Instantiate(this.prefabList[projIndex], this.transform.position, Quaternion.identity);
		projectile.GetComponent<Projectiles_Global>().StatsReceiver(this.gameObject, 3, this.dirX, this.dirY);
	}

	#region Provenance

	protected void Prov_Agent()
	{
		this.Prov_GetAttributes ();
		this.extractProvenance.NewAgentVertex (this.provIndentifier);
	}

	protected void Prov_GetAttributes()
	{
		this.extractProvenance.AddAttribute ("HP", this.stat_hp.ToString());
	}

	protected void Prov_Actions()
	{
		//Prov_Walk ();
	}

	void Prov_Walk()
	{
		Prov_GetAttributes();
		this.extractProvenance.NewActivityVertex("Walking");
		this.extractProvenance.HasInfluence(this.provIndentifier);
	}

	public void Prov_TakeDamage(GameObject enemy, float damageAmount)
	{
		Characters_Enemies characters_enemies = enemy.GetComponent<Characters_Enemies>(); 
		string infID = characters_enemies.Prov_Attack(damageAmount);
		this.Prov_TakeDamage(infID);
	}

	public void Prov_TakeDamage(string infID)
	{
		this.Prov_GetAttributes();
		this.extractProvenance.NewActivityVertex("Being Hit");
		// Check Influence
		this.extractProvenance.HasInfluence_ID(infID);
	}

	public void Prov_TakeDamage()
	{
		this.Prov_GetAttributes();
		this.extractProvenance.NewActivityVertex("Being Hit");
		// Check Influence
		this.extractProvenance.HasInfluence(this.provIndentifier + "Damage");
	}

	#endregion

}

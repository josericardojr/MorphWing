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
	protected string provIndentifier;
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
		projectile.GetComponent<Projectiles_Global>().StatsReceiver(this.gameObject, 3, passDirX, passDirY, this.GetInstanceID(), this.provIndentifier);
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

	protected void Prov_GetAttributes()
	{
		this.extractProvenance.AddAttribute ("HP", this.stat_hp.ToString());
        this.extractProvenance.AddAttribute("Speed", this.stat_speed.ToString());
        this.extractProvenance.AddAttribute("Last", this.lastHitBy);
    }


	public void Prov_TakeDamage(float instanceID, float damageAmount)
    {
        Prov_GetAttributes();
        string infID = instanceID.ToString();
		this.Prov_TakeDamage(infID);
	}

    protected void Prov_Heal()
    {
        Prov_GetAttributes();
        this.extractProvenance.NewActivityVertex("Heal");
        this.extractProvenance.HasInfluence(this.provIndentifier);
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
		this.extractProvenance.NewActivityVertex("Being Hit");
        // Check Influence
        this.extractProvenance.HasInfluence(this.lastHitBy);
        this.extractProvenance.HasInfluence_ID(infID);
    }

	public void Prov_TakeDamage()
	{
		this.Prov_GetAttributes();
		this.extractProvenance.NewActivityVertex("Being Hit");
		// Check Influence
		this.extractProvenance.HasInfluence(this.provIndentifier + "Damage");
	}

    public void Prov_PowerUp()
    {
        Prov_GetAttributes();
        this.extractProvenance.NewActivityVertex("PowerUp");
        this.extractProvenance.HasInfluence(this.provIndentifier);
        this.extractProvenance.provenance.Save("info");
    }

    #endregion

}

using UnityEngine;
using System.Collections;

public class Characters_Enemies : Characters_Global
{
	protected Characters_Player player;
    protected Managers_Spawn spawnManager;
	protected Vector2 initDir;
	[SerializeField]
	protected int contactDamage;

	#region GETS & SETS

	public Vector2 InitDir
	{
		get{return this.initDir;}
		set{this.initDir = value;}
	}

	#endregion

	new protected void Start()
	{
		base.Start();
		Prov_Agent ();
        this.spawnManager = GameObject.Find("SpawnManager").GetComponent<Managers_Spawn>();
		this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Characters_Player>();
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if (c.CompareTag ("Player"))
		{
			if(!c.GetComponent<Characters_Player>().Invincible)
				c.GetComponent<Characters_Global> ().GetDamaged(this.gameObject, this.contactDamage);
		}
	}

    protected override void CheckIfAlive()
    {
        if (this.temp_currHp <= 0)
            this.spawnManager.EnemyDecrease();
            base.CheckIfAlive();
    }

    public string Prov_Attack(float damageAmount)
	{
		Prov_GetAttributes ();
		this.extractProvenance.NewActivityVertex("Attacking", this.gameObject);
		this.extractProvenance.HasInfluence("Enemy");
		this.extractProvenance.GenerateInfluenceCE("PlayerDamage", this.GetInstanceID().ToString(), "Health (Player)", (-damageAmount).ToString(), 1, Time.time + 5);
		return this.GetInstanceID().ToString();
	}

}

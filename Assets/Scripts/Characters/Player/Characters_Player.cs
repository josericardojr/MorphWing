using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Characters_Player : Characters_Global 
{
    [SerializeField]
    GameObject cooldownIndicator;
	[SerializeField]
	ScoreManager managers_score;
	[SerializeField]
	Managers_Spawn managers_spawn;
	[SerializeField]
	Image hud_healthFill;
	[SerializeField]
	Text hud_retryText;
	[SerializeField]
	protected float invincibleTime;
	protected bool invincible, slowdown, canTakeDamage;
	[SerializeField]
	List<float> cooldownList = new List<float>();
	List<float> currCooldown = new List<float>();

	[HideInInspector]
	public bool invertControl;

    float damageMultiplier = 1;
    bool attackBuff = false;

	#region GETS & SETS

    public float DamageMultiplier
    {
        get { return this.damageMultiplier; }
        set { this.damageMultiplier = value; }
    }

    public bool AttackBuff
    {
        get { return this.attackBuff; }
        set { this.attackBuff = value; }
    }

	public bool Invincible
	{
		get{return this.invincible;}
		set{this.invincible = value;}
	}

	public bool CanTakeDamage
	{
		get { return this.canTakeDamage; }
		set { this.canTakeDamage = value; }
	}

	#endregion

	new void Start () 
	{
		SetCooldowns();
		base.Start();
		Prov_Agent ();
        this.GetComponent<TrailRenderer>().enabled = false;
	}

	void SetCooldowns()
	{
		for (int i = 0; i < this.cooldownList.Count; i++)
		{
			this.currCooldown.Add(0);
		}
	}

	void Update () 
	{
		Movement ();
		if (Input.GetButton("Fire1") && this.currCooldown[0] == 0)
			ShootProjectile(0, 0, 1);
		if (Input.GetButton("Fire2") && this.currCooldown[1] == 0)
			ShootProjectile(1, this.dirX, this.dirY);
		CooldownRun();
		ShotSlowdown();
	}

	public override void GetDamaged(float instanceID, string objLabel, float damage)
	{
		base.GetDamaged(instanceID, objLabel, damage);
        GameObject.FindObjectOfType<BingApplication>().FirstTest();
        this.animator.SetInteger("Invincibility", 1);
		this.invincible = true;
		UpdateUI();
		Invoke("StopInvincibility", this.invincibleTime);
	}

	public void InvicibleTouch(float instanceID, string objLabel)
	{
		string infID = instanceID.ToString();
		Prov_GetAttributes();
		this.extractProvenance.NewActivityVertex("Damage_Neutralized");
		// Check Influence
		this.extractProvenance.HasInfluence("Invencibilidade");
		this.extractProvenance.HasInfluence_ID(infID);
		//this.extractProvenance.HasInfluence_ID(instanceID.ToString());
	}

	void CooldownRun()
	{
		for (int i = 0; i < this.currCooldown.Count; i++)
            if (this.currCooldown[i] > 0)
                this.currCooldown[i] -= Time.deltaTime;
            else
            {
                this.currCooldown[i] = 0;
                if (i == 1)
                    this.cooldownIndicator.SetActive(true);
            }
	}

	void StopInvincibility()
	{	
		this.animator.SetInteger("Invincibility", -1);
		this.invincible = false;
	}

	protected override void ShootProjectile(int projIndex, int passDirX, int passDirY)
	{
		base.ShootProjectile(projIndex, passDirX, passDirY);
		this.currCooldown[projIndex] = this.cooldownList[projIndex];
        if (projIndex == 1)
            this.cooldownIndicator.SetActive(false);
	}

	void Movement()
	{
		if (!this.invertControl)
		{
			this.dirX = (int)Input.GetAxisRaw("Horizontal");
			this.dirY = (int)Input.GetAxisRaw("Vertical");
		}
		else
		{
			this.dirX = -(int)Input.GetAxisRaw("Horizontal");
			this.dirY = -(int)Input.GetAxisRaw("Vertical");
		}
		if (!Input.GetButton ("Fix"))
			MovementCall();
		else
			this.rigidbody.velocity = new Vector2 (0, 0);
	}

	void UpdateUI()
	{
		this.hud_healthFill.fillAmount = (float)this.temp_currHp / (float)this.stat_hp;
	}

    public override void CheckIfAlive(float instanceID)
	{
		if (this.temp_currHp <= 0)
		{
			this.hud_retryText.enabled = true;
			this.managers_spawn.Deactivated = true;
            base.CheckIfAlive(instanceID);

            if (!this.balanceApplier.DontApplyBalance)
            {
                SaveProvenance();
            }

            this.managers_score.StopTimer();
            FindObjectOfType<Managers_WriteText>().WriteResults();
        }
	}


    public void SaveProvenance()
    {
        string nameProv = "info_" + GetDate();
        try
        {
            this.extractProvenance.provenance.Save(nameProv);
            /*
            AccessPython ac = FindObjectOfType<AccessPython>();

            if (ac != null)
            {
                ac.GetChanges(nameProv);
            }*/
        }
        catch (System.Exception e)
        {
            print(e.Message);
        }
    }

    public static string GetDate()
    {
        System.DateTime time = System.DateTime.Now;
        return (time.Day + "." + time.Month + "." + time.Year + ";" + time.Hour + "." + time.Minute);
    }

    protected override void Prov_GetAttributes()
	{
		base.Prov_GetAttributes();
		if(this.currCooldown[1] != null)
			this.extractProvenance.AddAttribute ("WeaponCD", this.currCooldown[1].ToString());
		else
			this.extractProvenance.AddAttribute ("WeaponCD", "0");
	}

	void ShotSlowdown()
	{
        if (Input.GetButton("Fire1"))
			this.stat_speed = 1.5f;
        else
			this.stat_speed = 3;
	}

	public void Heal(string infID)
	{
		this.temp_currHp += 2;
		if (this.temp_currHp > this.stat_hp)
			this.temp_currHp = this.stat_hp;
		Prov_Heal(infID);
		UpdateUI();
	}

}
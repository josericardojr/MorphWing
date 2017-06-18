using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Characters_Player : Characters_Global 
{
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
	protected bool invincible, slowdown;
    [SerializeField]
    List<float> cooldownList = new List<float>();
    List<float> currCooldown = new List<float>();

    [HideInInspector]
    public bool invertControl;

    #region GETS & SETS

    public bool Invincible
	{
		get{return this.invincible;}
		set{this.invincible = value;}
	}

	#endregion

	new void Start () 
	{
        SetCooldowns();
		base.Start();
		Prov_Agent ();
	}
	
    void SetCooldowns()
    {
        for (int i = 0; i < this.cooldownList.Count; i++)
        {
            this.currCooldown.Add(0);
            print("");
        }
    }

	void Update () 
	{
		Movement();
		if (Input.GetKey (KeyCode.J) && this.currCooldown[0] == 0)
			ShootProjectile(0, 0, 1);
        if (Input.GetKey (KeyCode.K) && this.currCooldown[1] == 0)
            ShootProjectile(1, this.dirX, this.dirY);
        CooldownRun();
        ShotSlowdown();
    }

	public override void GetDamaged(float instanceID, string objLabel, float damage)
    {
        base.GetDamaged(instanceID, objLabel, damage);
		this.animator.SetInteger("Invincibility", 1);
		this.invincible = true;
        UpdateUI();
        Invoke("StopInvincibility", this.invincibleTime);
	}

	public void InvicibleTouch(float instanceID, string objLabel)
    {
        Prov_GetAttributes();
		this.extractProvenance.NewActivityVertex("Blink Dodge");
        // Check Influence
        this.extractProvenance.HasInfluence(objLabel);
		this.extractProvenance.HasInfluence_ID(infID);
    }

    void CooldownRun()
    {
        for (int i = 0; i < this.currCooldown.Count; i++)
            if (this.currCooldown[i] > 0)
                this.currCooldown[i] -= Time.deltaTime;
            else
                this.currCooldown[i] = 0;
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
		MovementCall();
	}

    void UpdateUI()
    {
        this.hud_healthFill.fillAmount = (float)this.temp_currHp / (float)this.stat_hp;
    }

    protected override void CheckIfAlive()
    {
        if (this.temp_currHp <= 0)
        {
            this.hud_retryText.enabled = true;
            this.managers_spawn.Deactivated = true;
            this.managers_score.StopTimer();
            this.extractProvenance.provenance.Save("info");
        }
        base.CheckIfAlive();
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
        if (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.K))
            this.stat_speed = 1.5f;
        else if (!Input.GetKey(KeyCode.J) && !Input.GetKey(KeyCode.K))
            this.stat_speed = 3;
    }

    public void Heal()
    {
        this.temp_currHp += 2;
        if (this.temp_currHp > this.stat_hp)
            this.temp_currHp = this.stat_hp;
        Prov_Heal();
        UpdateUI();
    }

}
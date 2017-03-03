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
	protected bool invincible;
    [SerializeField]
    List<float> cooldownList = new List<float>();
    List<float> currCooldown = new List<float>();

    #region GETS & SETS

    public bool Invincible
	{
		get{return this.invincible;}
		set{this.invincible = value;}
	}

	#endregion

	new void Start () 
	{
		base.Start();
		Prov_Agent ();
        SetCooldowns();
	}
	
    void SetCooldowns()
    {
        for (int i = 0; i < this.cooldownList.Count; i++)
            this.currCooldown.Add(0);
    }

	void Update () 
	{
		Prov_Actions ();
		Movement();
		if (Input.GetKeyDown (KeyCode.J) && this.currCooldown[0] == 0)
			ShootProjectile(0, 0, 1);
        if (Input.GetKeyDown(KeyCode.K) && this.currCooldown[1] == 0)
            ShootProjectile(1, 0, 1);
        CooldownRun();
    }

	public override void GetDamaged(GameObject attacker, float damage)
	{
		base.GetDamaged(attacker, damage);
		this.animator.SetInteger("Invincibility", 1);
		this.invincible = true;
        UpdateUI();
        Invoke("StopInvincibility", this.invincibleTime);
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
		this.dirX = (int)Input.GetAxisRaw ("Horizontal");
		this.dirY = (int)Input.GetAxisRaw ("Vertical");
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

}
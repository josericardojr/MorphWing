using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Characters_Player : Characters_Global 
{
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
			ShootProjectile(0);
        if (Input.GetKeyDown(KeyCode.K) && this.currCooldown[1] == 0)
            ShootProjectile(1);
        CooldownRun();
        //this.extractProvenance.provenance.Save ("info");
    }

	public override void GetDamaged(GameObject attacker, float damage)
	{
		base.GetDamaged(attacker, damage);
		this.animator.SetInteger("Invincibility", 1);
		this.invincible = true;
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

    protected override void ShootProjectile(int projIndex)
    {
        base.ShootProjectile(projIndex);
        this.currCooldown[projIndex] = this.cooldownList[projIndex];
    }

    void Movement()
	{
		this.dirX = (int)Input.GetAxisRaw ("Horizontal");
		this.dirY = (int)Input.GetAxisRaw ("Vertical");
		MovementCall();
	}

}
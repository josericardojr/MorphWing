using UnityEngine;
using System.Collections;

public class Characters_Player : Characters_Global 
{
	[SerializeField]
	protected float invincibleTime;
	protected bool invincible;

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
	}
		
	void Update () 
	{
		Prov_Actions ();
		Movement();
		if (Input.GetKeyDown (KeyCode.Space))
			ShootProjectile(0);
		//this.extractProvenance.provenance.Save ("info");
	}

	public override void GetDamaged(GameObject attacker, int damage)
	{
		base.GetDamaged(attacker, damage);
		this.animator.SetInteger("Invincibility", 1);
		this.invincible = true;
		Invoke("StopInvincibility", this.invincibleTime);
	}

	void StopInvincibility()
	{	
		this.animator.SetInteger("Invincibility", -1);
		this.invincible = false;
	}

	void Movement()
	{
		this.dirX = (int)Input.GetAxisRaw ("Horizontal");
		this.dirY = (int)Input.GetAxisRaw ("Vertical");
		MovementCall();
	}

}
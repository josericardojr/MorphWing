using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles_Global : MonoBehaviour
{
    BalanceApplier balanceApplier;
    ScoreManager scoreManager;
	Managers_Spawn managers_spawn;
	new protected Rigidbody2D rigidbody;
	GameObject shooter;
	protected float receivedX, receivedY, maxOffsetX, maxOffsetY, shooterInstanceID;
    bool upEffect = false;
	[SerializeField]
	List<GameObject> prefabList = new List<GameObject>();
	[SerializeField]
	float damage, minPlayerDamage, maxPlayerDamage;
	[SerializeField]
	float speed;

    protected int prov_id;
    [SerializeField]
    int hitScore;

	string shooterLabel;


	protected void Start()
    {
        this.balanceApplier = GameObject.Find("Provenance").GetComponent<BalanceApplier>();
        if (this.CompareTag("Player_Shot"))
        {
            this.scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
            this.damage *= this.balanceApplier.DamageModifier;
        }
        if (this.upEffect)
            this.GetComponent<Animator>().SetTrigger("Crazy");

        if (this.CompareTag("Enemy_Shot"))
            this.speed *= this.balanceApplier.difficultyMultipliers[this.prov_id];
        
		this.rigidbody = this.GetComponent<Rigidbody2D>();
		this.managers_spawn = GameObject.Find("SpawnManager").GetComponent<Managers_Spawn>();
		this.maxOffsetX = this.managers_spawn.MaxOffsetX;
		this.maxOffsetY = this.managers_spawn.MaxOffsetY;
	}

	public void StatsReceiver(GameObject shooterObj, float damageValue, float dirXValue, float dirYValue, float instanceID, string objLabel)
	{
        if (shooterObj.GetComponent<Characters_Enemies>())
            this.prov_id = shooterObj.GetComponent<Characters_Enemies>().ProvIdNum;
		this.tag = shooterObj.tag.Split('_')[0] + "_Shot";
		this.shooterLabel = objLabel;
		this.shooter = shooterObj;
		//this.damage = damageValue;
		this.receivedX = dirXValue;
		this.receivedY = dirYValue;
		this.shooterInstanceID = instanceID;
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if(c.CompareTag("Enemy") && this.CompareTag("Player_Shot") ||
			c.CompareTag("Player") && this.CompareTag("Enemy_Shot"))
		{
			c.GetComponent<Characters_Global>().GetDamaged(this.GetComponent<Collider2D>().GetInstanceID(), this.shooterLabel, this.damage);
            if (this.CompareTag("Player_Shot"))
                this.scoreManager.AddScore(this.hitScore);
		}
	}

	protected void CallMovement()
	{
		if(this.tag == "Player")
			print(receivedX + " " + receivedY);
		this.rigidbody.velocity = new Vector2(this.speed * this.receivedX, 
			this.speed * this.receivedY);
	}

	protected void ShootProjectile(int projIndex, float directionX, float directionY)
	{
		GameObject projectile = GameObject.Instantiate(this.prefabList[projIndex], this.transform.position, Quaternion.identity);
		projectile.GetComponent<Projectiles_Global>().StatsReceiver(this.shooter, 3, directionX, directionY, projectile.GetComponent<Collider2D>().GetInstanceID(), this.shooterLabel);
        if (this.upEffect)
            projectile.GetComponent<Projectiles_Global>().upEffect = true;
	}

	protected void DestroyOffScreen()
	{
		if ((this.transform.position.y > this.maxOffsetY ||
			this.transform.position.y < -this.maxOffsetY ||
			this.transform.position.x > this.maxOffsetX ||
			this.transform.position.x < -this.maxOffsetX) 
			&& this.maxOffsetY != 0 && this.maxOffsetX != 0)
		{
			Destroy(this.gameObject);
		}
	}

	#region GETS & SETS

    public float Damage
    {
        get { return this.damage; }
        set { this.damage = value; }
    }

	public bool UpEffect
	{
        get { return this.upEffect; }
        set { this.upEffect = value; }
	}

	#endregion

}

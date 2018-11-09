using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Characters_Enemies : Characters_Global
{
    [SerializeField]
    GameObject popUpPrefab;
	protected Characters_Player player;
	protected Managers_Spawn spawnManager;
	protected Vector2 initDir;
	protected bool canDestroyOffScreen = false;
	[SerializeField]
	protected int provIdNum, contactDamage, scoreReward, timeReward;
    [SerializeField]
	protected float maxOffsetX, maxOffsetY;

	#region GETS & SETS

	public Vector2 InitDir
	{
		get{return this.initDir;}
		set{this.initDir = value;}
	}

	public float MaxOffsetX
	{
		get { return this.maxOffsetX; }
		set { this.maxOffsetX = value; }
	}

	public float MaxOffsetY
	{
		get { return this.maxOffsetY; }
		set { this.maxOffsetY = value; }
	}

    public int ProvIdNum
    {
        get { return this.provIdNum; }
        set { this.provIdNum = value; }
    }

	#endregion

	new protected void Start()
	{
        this.initialInvic = true;
        base.Start();
        this.animator.SetInteger("Invincibility", 1);
        Prov_Agent();
        this.spawnManager = GameObject.Find("SpawnManager").GetComponent<Managers_Spawn>();
        this.scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
		if(GameObject.FindGameObjectWithTag("Player") != null)
			this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Characters_Player>();
	}

	void OnTriggerStay2D(Collider2D c)
	{
        if (c.CompareTag("Player") && !this.initialInvic)
		{
            if (!c.GetComponent<Characters_Player>().Invincible)
            {
                Prov_EnemyAttack(this.contactDamage);
                c.GetComponent<Characters_Global>().GetDamaged(this.GetInstanceID(), this.provIndentifier, this.contactDamage);
            }
		}
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if (c.CompareTag ("Player") && !this.initialInvic)
		{
			if (c.GetComponent<Characters_Player> ().Invincible)
			{
				c.GetComponent<Characters_Player>().InvicibleTouch(this.GetInstanceID(), this.provIndentifier);
			}
		}
	}

	protected void SetDirection()
	{
		if (this.dirX != this.initDir.x ||
			this.dirY != this.initDir.y)
		{
			this.dirX = (int)this.initDir.x;
			this.dirY = (int)this.initDir.y;
		}
	}

    public override void CheckIfAlive(float instanceID)
    {
        if (this.temp_currHp <= 0 && !this.dead)
        {
            this.dead = true;
            this.scoreManager.EnemyKills[this.provIdNum]++;
            this.scoreManager.AddScore(this.scoreReward);
            this.scoreManager.TimeCurrent += this.timeReward;
            GameObject.Instantiate(this.popUpPrefab, Camera.main.WorldToScreenPoint(this.transform.position), Quaternion.identity, GameObject.Find("Canvas").transform).transform.GetChild(1).GetComponent<Text>().text = "+" + this.timeReward.ToString();
        }
        base.CheckIfAlive(instanceID);
	}

	protected void CanDestroyOutOfScreen ()
    {
        if (!this.canDestroyOffScreen && this.transform.position.x < 5.3f && this.transform.position.x > -5.3f &&
            this.transform.position.y > -5.3f && this.transform.position.y < 5.3f)
        {
            this.canDestroyOffScreen = true;
            this.animator.SetInteger("Invincibility", -1);
            this.spawnManager.EnemySpawns[this.provIdNum]++;
            this.initialInvic = false;
        }
	}

	protected void DestroyOffScreen()
	{
        if(this.canDestroyOffScreen)
		    if (this.transform.position.y > this.maxOffsetY ||
			    this.transform.position.y < -this.maxOffsetY ||
			    this.transform.position.x > this.maxOffsetX ||
			    this.transform.position.x < -this.maxOffsetX)
            {
			    Destroy(this.gameObject);
		    }
	}

}

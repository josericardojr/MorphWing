﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles_Global : MonoBehaviour
{
    Managers_Spawn managers_spawn;
	new protected Rigidbody2D rigidbody;
	GameObject shooter;
    protected float receivedX, receivedY, maxOffsetX, maxOffsetY;
    [SerializeField]
    List<GameObject> prefabList = new List<GameObject>();
	[SerializeField]
	float damage;
    [SerializeField]
    float speed;

    protected void Start()
	{
		this.rigidbody = this.GetComponent<Rigidbody2D>();
        this.managers_spawn = GameObject.Find("SpawnManager").GetComponent<Managers_Spawn>();
        this.maxOffsetX = this.managers_spawn.MaxOffsetX;
        this.maxOffsetY = this.managers_spawn.MaxOffsetY;
	}

	public void StatsReceiver(GameObject shooterObj, float damageValue, float dirXValue, float dirYValue)
	{
		this.tag = shooterObj.tag.Split('_')[0] + "_Shot";
		this.shooter = shooterObj;
		this.damage = damageValue;
		this.receivedX = dirXValue;
		this.receivedY = dirYValue;
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if(c.CompareTag("Enemy") && this.CompareTag("Player_Shot") ||
		   c.CompareTag("Player") && this.CompareTag("Enemy_Shot"))
		{
			c.GetComponent<Characters_Global>().GetDamaged(this.shooter, this.damage);
		}
	}

	protected void CallMovement()
	{
		this.rigidbody.velocity = new Vector2(this.speed * this.receivedX * 60 * Time.deltaTime, 
											  this.speed * this.receivedY * 60 * Time.deltaTime);
	}

    protected void ShootProjectile(int projIndex, float directionX, float directionY)
    {
        GameObject projectile = GameObject.Instantiate(this.prefabList[projIndex], this.transform.position, Quaternion.identity);
        projectile.GetComponent<Projectiles_Global>().StatsReceiver(this.shooter, 3, directionX, directionY);
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

}

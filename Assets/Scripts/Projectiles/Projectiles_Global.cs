using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles_Global : MonoBehaviour
{
	new protected Rigidbody2D rigidbody;
	GameObject shooter;
	protected int receivedX, receivedY, dirX, dirY;
	[SerializeField]
	int damage;
	[SerializeField]
	float speed;

	protected void Start()
	{
		this.rigidbody = this.GetComponent<Rigidbody2D>();
	}

	public void StatsReceiver(GameObject shooterObj, int damageValue, int dirXValue, int dirYValue)
	{
		this.tag = shooterObj.tag;
		this.shooter = shooterObj;
		this.damage = damageValue;
		this.receivedX = dirXValue;
		this.receivedY = dirYValue;
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if(c.CompareTag("Enemy") && this.CompareTag("Player") ||
		   c.CompareTag("Player") && this.CompareTag("Enemy"))
		{
			c.GetComponent<Characters_Global>().GetDamaged(this.shooter, this.damage);
		}
	}

	protected void CallMovement()
	{
		this.rigidbody.velocity = new Vector2(this.speed * this.dirX * 60 * Time.deltaTime, 
											  this.speed * this.dirY * 60 * Time.deltaTime);
	}
}

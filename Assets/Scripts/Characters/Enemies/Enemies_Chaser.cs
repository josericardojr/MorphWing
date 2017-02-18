using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_Chaser : Characters_Enemies
{
	new void Start()
	{
		base.Start();
	}

	void Update()
	{
		Chase();
		Movement();
	}

	void Chase()
	{
		Vector2 enemyPos = this.transform.position;
		Vector2 playerPos = this.player.transform.position;

		if(Mathf.Abs(enemyPos.x - playerPos.x) > 0.15f)
			this.dirX = Libraries_Formulas.ReturnDirection(enemyPos.x, playerPos.x);
		else 
			this.dirX = 0;

		if(Mathf.Abs(enemyPos.y - playerPos.y) > 0.15f)
			this.dirY = Libraries_Formulas.ReturnDirection(enemyPos.y, playerPos.y);
		else
			this.dirY = 0;
	}

	void Movement()
	{
		MovementCall();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles_RegularBullet : Projectiles_Global
{
	new void Start()
	{
		base.Start();
	}

	void Update()
	{
		CallMovement();
	}
}

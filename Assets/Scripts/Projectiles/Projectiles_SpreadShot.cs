using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles_SpreadShot : Projectiles_Global
{
    new void Start ()
    {
        base.Start();
        this.receivedY = 1;
        ShootProjectile(0, -0.5f, 1);
        ShootProjectile(0, 0.5f, 1);
    }
	
	void Update ()
    {
        CallMovement();	
	}
}

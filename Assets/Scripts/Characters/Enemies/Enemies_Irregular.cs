using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_Irregular : Characters_Enemies
{
	new void Start ()
    {
        base.Start();
        this.dirX = (int)initDir.x;
        this.dirY = (int)initDir.y;
        Invoke("Stop", Random.Range(0.7f, 1.3f));
    }
	
	void Update ()
    {
        MovementCall();
        DestroyOffScreen();
	}

    void Stop()
    {
        this.dirX = 0;
        this.dirY = 0;
        Invoke("Shoot", Random.Range(0.5f, 1));
    }

    void Shoot()
    {
        ShootProjectile(0, 1, 0);
        ShootProjectile(0, 0, 1);
        ShootProjectile(0, -1, 0);
        ShootProjectile(0, 0, -1);
        Invoke("GoBack", Random.Range(0.7f, 1.2f));
    }

    void GoBack()
    {
        this.dirX = -(int)initDir.x;
        this.dirY = -(int)initDir.y;
    }
}

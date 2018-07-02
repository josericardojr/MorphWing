using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_Irregular : Characters_Enemies
{
    [SerializeField]
    float prepareTime;

    public float PrepareTime
    {
        get { return this.prepareTime; }
    }

	new void Start ()
    {
        base.Start();
        this.dirX = (int)initDir.x;
        this.dirY = (int)initDir.y;
        Invoke("Stop", Random.Range(1.1f, 1.3f));
        this.Prov_UsingAttack(this.GetInstanceID().ToString());
    }
	
	void Update ()
    {
        CanDestroyOutOfScreen();
        MovementCall();
        DestroyOffScreen();
	}

    void Stop()
    {
        this.dirX = 0;
        this.dirY = 0;
        Invoke("Shoot", this.prepareTime / this.balanceApplier.difficultyMultipliers[this.provIdNum]);
    }

    void Shoot()
    {
        ShootProjectile(0, 1, 0);
        ShootProjectile(0, 0, 1);
        ShootProjectile(0, -1, 0);
        ShootProjectile(0, 0, -1);
        Invoke("GoBack", this.prepareTime / this.balanceApplier.difficultyMultipliers[this.provIdNum]);
        this.Prov_UsingAttack(this.GetInstanceID().ToString());
    }

    void GoBack()
    {
        this.dirX = -(int)initDir.x;
        this.dirY = -(int)initDir.y;
    }
}

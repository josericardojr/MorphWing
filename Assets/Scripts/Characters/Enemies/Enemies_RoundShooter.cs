using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_RoundShooter : Characters_Enemies
{
    [SerializeField]
    List<Vector2> shootDirOrder = new List<Vector2>();
    [SerializeField]
    float prepareTime;

    public float PrepareTime
    {
        get { return this.prepareTime; }
    }

    private bool startShooting;

    new void Start()
    {
        base.Start();
        this.Prov_UsingAttack(this.GetInstanceID().ToString());
    }

    void Update()
    {
        CanDestroyOutOfScreen();
        MovementCall();
        SetDirection();
        Slowdown();
    }
    
    void Slowdown()
    {
        if(this.stat_speed > 0)
            this.stat_speed -= Time.deltaTime * 0.5f;
        if (this.stat_speed < 0)
            this.stat_speed = 0;
        if(!this.startShooting && this.stat_speed == 0)
        {
            this.startShooting = true;
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        for (int i = 0; i < this.shootDirOrder.Count; i++)
        {
            yield return new WaitForSeconds(this.prepareTime / this.balanceApplier.difficultyMultipliers[this.provIdNum]);
            ShootProjectile(0, (int)this.shootDirOrder[i].x, (int)this.shootDirOrder[i].y);
            this.Prov_UsingAttack(this.GetInstanceID().ToString());
        }
        StartCoroutine(Shoot());
    }

}

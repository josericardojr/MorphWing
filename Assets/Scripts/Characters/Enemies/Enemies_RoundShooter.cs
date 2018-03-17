using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_RoundShooter : Characters_Enemies
{
    [SerializeField]
    List<Vector2> shootDirOrder = new List<Vector2>();

    private bool startShooting;

    new void Start()
    {
        base.Start();
    }

    void Update()
    {
        MovementCall();
        DestroyOffScreen();
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
            yield return new WaitForSeconds(0.5f);
            ShootProjectile(0, (int)this.shootDirOrder[i].x, (int)this.shootDirOrder[i].y);
            this.Prov_UsingAttack(this.GetInstanceID().ToString());
        }
        StartCoroutine(Shoot());
    }

}

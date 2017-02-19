using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles_HeavyBullet : Projectiles_Global {

    new void Start()
    {
        base.Start();
        this.receivedX = 0;
        this.receivedY = 1;
    }

    void Update()
    {
        CallMovement();
    }
}

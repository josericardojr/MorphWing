using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles_HeavyBullet : Projectiles_Global {

    new void Start()
    {
        base.Start();
        if (this.receivedY == 0 && this.receivedX == 0)
            this.receivedY = 1;
        
    }

    void Update()
    {
        CallMovement();
        DestroyOffScreen();
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_Straight : Characters_Enemies
{
    new void Start()
    {
        base.Start();
        this.Prov_UsingAttack(this.GetInstanceID().ToString());
    }

    void Update()
    {
        MovementCall();
        DestroyOffScreen();
        SetDirection();
    }

}

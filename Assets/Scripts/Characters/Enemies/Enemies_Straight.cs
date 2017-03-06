using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_Straight : Characters_Enemies
{
    new void Start()
    {
        base.Start();
    }

    void Update()
    {
        MovementCall();
        DestroyOffScreen();
        SetDirection();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects_Destroyable : MonoBehaviour 
{
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}

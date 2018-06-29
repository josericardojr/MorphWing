using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyProvenance : MonoBehaviour 
{
	void Start ()
    {
        if (GameObject.Find("Balance"))
        {
            Destroy(GameObject.Find("Balance"));
        }
	}
}

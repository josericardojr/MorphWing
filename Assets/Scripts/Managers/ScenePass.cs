using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePass : MonoBehaviour 
{
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (AccessPython.Instance.Ready)
            {
                SceneManager.LoadScene(1); 
            }
        }
	}
}

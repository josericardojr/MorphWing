using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private GameObject textTime;

    private float timeCurrent;

    public float TimeCurrent
    {
    	get{ return this.timeCurrent; }
    }

    Thread threadTime;
    private static volatile bool running;

    void Start ()
    {
        this.timeCurrent = 0;
        this.textTime = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).gameObject;
        running = true;
	}

    void Update()
	{
        this.timeCurrent += Time.deltaTime;
        this.textTime.GetComponent<Text>().text = ((int)this.timeCurrent).ToString();
    }
    
    public void SaveTime()
    {
        PlayerPrefs.SetFloat(PlayerPrefsKey.keyScore, this.timeCurrent);
    }

    public void StopTimer()
    {
        running = false;
    }
}

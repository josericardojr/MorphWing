using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    private GameObject textTime;

    private int timeCurrent;

    Thread threadTime;
    private static volatile bool running;

    void Start ()
    {
        this.timeCurrent = -1;
        this.textTime = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).gameObject;
        this.threadTime = new Thread(Temporizador);
        this.threadTime.Start();
        running = true;

	}

    public void Temporizador()
    {
        while(running)
        {
            this.timeCurrent++;
            System.Threading.Thread.Sleep(1000);
        }
    }

    void Update()
    {
        this.textTime.GetComponent<Text>().text = this.timeCurrent.ToString();
    }
    
    public void SaveTime()
    {
        PlayerPrefs.SetInt(PlayerPrefsKey.keyScore, this.timeCurrent);
    }

    public void StopTimer()
    {
        running = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private GameObject textTime;
    [SerializeField]
    Text gameOverTimerText;

    private float timeCurrent, gameOverTime = 5;

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
        if(!running)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
                SceneManager.LoadScene(0);
            this.gameOverTime -= Time.deltaTime / 1.8f;
            if(this.gameOverTime <= 0)
                SceneManager.LoadScene(0);
            this.gameOverTimerText.text = ((int)gameOverTime + 1).ToString();
        }
    }
    
    public void SaveTime()
    {
        PlayerPrefs.SetFloat(PlayerPrefsKey.keyScore, this.timeCurrent);
    }

    public void StopTimer()
    {
        running = false;
        this.gameOverTimerText.enabled = true;
    }
}

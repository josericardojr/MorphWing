﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    Characters_Player player;
    List<int> itemsGot = new List<int>();

    private GameObject textTime;
    [SerializeField]
    Text gameOverTimerText, scoreText, bestText;
    GameObject balanceApplier;

    List<int> enemyKills = new List<int>();

    [SerializeField]
    float timeCurrent;
    float gameOverTime = 5, elapsedTime;
    
    int score;

    public int Score
    {
        get { return this.score; }
    }

    public float TimeCurrent
    {
    	get{ return this.timeCurrent; }
    	set{ this.timeCurrent = value; }
    }

    public List<int> EnemyKills
    {
        get { return this.enemyKills; }
        set { this.enemyKills = value; }
    }

    public float ElapsedTime
    {
        get { return this.elapsedTime; }
    }

    public List<int> ItemsGot
    {
        get { return this.itemsGot; }
        set { this.itemsGot = value; }
    }

    private static volatile bool running;

    void Awake ()
    {
        balanceApplier = GameObject.Find("Balance").gameObject;
        this.bestText.text = balanceApplier.GetComponent<BalanceApplier>().BestScore.ToString();
        this.balanceApplier.GetComponent<BalanceApplier>().ReAwake();
        this.textTime = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).gameObject;
        running = true;
        for (int i = 0; i < 4; i++)
        {
            this.enemyKills.Add(0);
        }
        for (int i = 0; i < 4; i++)
            itemsGot.Add(0);
	}

    void Start()
    {
        this.balanceApplier.GetComponent<BalanceApplier>().Restart();
    }

    void Update()
	{
        if(this.timeCurrent > 0)
            this.timeCurrent -= Time.deltaTime;
        if (this.timeCurrent < 0)
        {
            this.timeCurrent = 0;
            if (player.Temp_CurrHp > 0)
            {
                this.player.Temp_CurrHp = 0;
                this.player.CheckIfAlive(player.GetInstanceID());
            }
        }
        if(running)
            this.elapsedTime += Time.deltaTime;
        this.textTime.GetComponent<Text>().text = ((int)this.timeCurrent).ToString();
        if(!running)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(this.balanceApplier);
                SceneManager.LoadScene(0);
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if(this.score > this.balanceApplier.GetComponent<BalanceApplier>().BestScore)
                    this.balanceApplier.GetComponent<BalanceApplier>().BestScore = this.score;
                SceneManager.LoadScene(1);
            }
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

    public void AddScore(int amount)
    {
        this.score += amount;
        this.scoreText.text = score.ToString();
    }

    public void StopTimer()
    {
        this.score += 110 * (int)elapsedTime;
        running = false;
        this.scoreText.text = score.ToString();
        this.gameOverTimerText.enabled = true;
    }
}

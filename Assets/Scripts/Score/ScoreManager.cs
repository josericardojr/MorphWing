using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    Characters_Player player;

    private GameObject textTime;
    [SerializeField]
    Text gameOverTimerText, scoreText;
    GameObject balanceApplier;

    [SerializeField]
    float timeCurrent;
    float gameOverTime = 5, elapsedTime;
    
    int score;

    public float TimeCurrent
    {
    	get{ return this.timeCurrent; }
    	set{ this.timeCurrent = value; }
    }

    public float ElapsedTime
    {
        get { return this.elapsedTime; }
    }

    private static volatile bool running;

    void Start ()
    {
        balanceApplier = GameObject.Find("Provenance").gameObject;
        this.textTime = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).gameObject;
        running = true;
	}

    void Update()
	{
        if(this.timeCurrent > 0)
            this.timeCurrent -= Time.deltaTime;
        if (this.timeCurrent < 0)
        {
            this.timeCurrent = 0;
            this.player.Temp_CurrHp = 0;
            this.player.CheckIfAlive(player.GetInstanceID());
        }
        if(running)
            this.elapsedTime += Time.deltaTime;
        this.textTime.GetComponent<Text>().text = ((int)this.timeCurrent).ToString();
        if(!running)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
                Destroy(this.balanceApplier);
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
        this.gameOverTimerText.enabled = true;
    }
}

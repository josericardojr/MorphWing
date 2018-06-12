using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Managers_WriteText : MonoBehaviour 
{
    [SerializeField]
    ScoreManager scoreManager;
    List<int> itemsGot = new List<int>();

    void Start()
    {
        this.scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        for (int i = 0; i < 4; i++)
            itemsGot.Add(0);
    }

    public List<int> ItemsGot
    {
        get { return this.itemsGot; }
        set { this.itemsGot = value; }
    }

	public void WriteResults () 
    {
        string path = "Assets/Resources/test.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("User: " + this.GetInstanceID() + " " + DateTime.Now.ToString() + Environment.NewLine +
                         "Time Survived: " + this.scoreManager.ElapsedTime + Environment.NewLine +
                         "Score: " + this.scoreManager.Score + Environment.NewLine +
                         "Straight: " + this.scoreManager.EnemyKills[0] + Environment.NewLine + 
                         "Chaser: " + this.scoreManager.EnemyKills[1] + Environment.NewLine + 
                         "Round: " + this.scoreManager.EnemyKills[2] + Environment.NewLine +
                         "Irregular: " + this.scoreManager.EnemyKills[3] + Environment.NewLine +
                         "PowerUp: " + this.itemsGot[0] + Environment.NewLine + 
                         "PowerDown: " + this.itemsGot[1] + Environment.NewLine +
                         "SpeedUp: " + this.itemsGot[2] + Environment.NewLine +
                         "SpeedDown: " + this.itemsGot[3]);
        writer.Close();
	}
	
}

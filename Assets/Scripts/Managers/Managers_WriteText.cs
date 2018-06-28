using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Managers_WriteText : MonoBehaviour 
{
    ScoreManager scoreManager;

	public void WriteResults ()
    {
        if(scoreManager == null)
            this.scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        BalanceApplier balanceApplier = FindObjectOfType<BalanceApplier>();
        string path = Application.dataPath + @"/Resources/" + this.GetComponent<BalanceApplier>().RandomID.ToString() + ".txt";
        bool firstLine = false;
        if (!File.Exists(path))
            firstLine = true;
        string text = "";
        StreamWriter writer = new StreamWriter(path, true);

        if(firstLine)
            text += "Date" + ";" +
                         "Time Survived" + ";" +
                         "Score" + ";" +
                         "Straight" + ";" +
                         "Chaser" + ";" +
                         "Round" + ";" +
                         "Irregular" + ";" +
                         "PowerUp" + ";" +
                         "PowerDown" + ";" +
                         "SpeedUp" + ";" +
                         "SpeedDown" + ";" +
                         "Enemy1" + ";" +
                         "Enemy2" + ";" +
                         "Enemy3" + ";" +
                         "Enemy4" + ";" +
                         "Item1" + ";" +
                         "Item2" + ";" +
                         "Item3" + ";" +
                         "Item4" + ";" +
                         "PlayerDamage" + ";" + Environment.NewLine;

        text += DateTime.Now.ToString() + ";" +
                         this.scoreManager.ElapsedTime + ";" +
                         this.scoreManager.Score + ";" +
                         this.scoreManager.EnemyKills[0] + ";" +
                         this.scoreManager.EnemyKills[1] + ";" +
                         this.scoreManager.EnemyKills[2] + ";" +
                         this.scoreManager.EnemyKills[3] + ";" +
                         this.scoreManager.ItemsGot[0] + ";" +
                         this.scoreManager.ItemsGot[1] + ";" +
                         this.scoreManager.ItemsGot[2] + ";" +
                         this.scoreManager.ItemsGot[3] + ";" +
                         balanceApplier.difficultyMultipliers[0] + ";" +
                         balanceApplier.difficultyMultipliers[1] + ";" +
                         balanceApplier.difficultyMultipliers[2] + ";" +
                         balanceApplier.difficultyMultipliers[3] + ";" +
                         balanceApplier.itemDistances[0] + ";" +
                         balanceApplier.itemDistances[1] + ";" +
                         balanceApplier.itemDistances[2] + ";" +
                         balanceApplier.itemDistances[3] + ";" +
                         balanceApplier.damageModifier + ";";
        /*
        if (balanceApplier)
        {   
            string[] enemy = new string[balanceApplier.ChangedDifficultyMultiplier.Length];

            for (int i = 0; i < enemy.Length; i++)
            {
                enemy[i] = "enemy " + (i + 1);
            }

            for (int i = 0; i < balanceApplier.ChangedDifficultyMultiplier.Length; i++)
            {
                text += enemy[i % enemy.Length] + ": " + balanceApplier.ChangedDifficultyMultiplier[i] + Environment.NewLine;
            }

            string[] item = new string[balanceApplier.ChangedItemDistances.Length];

            for (int i = 0; i < item.Length; i++)
            {
                item[i] = "item " + (i + 1);
            }           

            for (int i = 0; i < balanceApplier.ChangedItemDistances.Length; i++)
            {
                text += item[i % item.Length] + ": " + balanceApplier.ChangedItemDistances[i] + Environment.NewLine;
            }

            string tag = "Player Damage";
            text += tag + ": " + balanceApplier.ChangedDamageModifier + Environment.NewLine;

        }
        else
        {
            print("Dont find BalanceApplier");
        }*/

        writer.WriteLine(text);
        writer.Close();
	}
	
}

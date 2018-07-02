using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Managers_WriteText : MonoBehaviour 
{
    ScoreManager scoreManager;
    Managers_Spawn spawnManager;
    SpawnItemManager spawnItemManager;
    [SerializeField]
    List<GameObject> enemyPrefabs;
    [SerializeField]
    GameObject bulletPrefab;

	public void WriteResults ()
    {
        if(scoreManager == null)
            this.scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        if (spawnManager == null)
            this.spawnManager = GameObject.Find("SpawnManager").GetComponent<Managers_Spawn>();
        if (spawnItemManager == null)
            this.spawnItemManager = GameObject.Find("SpawnManager").GetComponent<SpawnItemManager>();

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
                         "Straight Spawns" + ";" +
                         "Straight Kills" + ";" +
                         "Chaser Spawns" + ";" +
                         "Chaser Kills" + ";" +
                         "Round Spawns" + ";" +
                         "Round Kills" + ";" +
                         "Boomerang Spawns" + ";" +
                         "Boomerang Kills" + ";" +
                         "PowerUp Spawns" + ";" +
                         "PowerUp Got" + ";" +
                         "PowerDown Spawns" + ";" +
                         "PowerDown Got" + ";" +
                         "SpeedUp Spawns" + ";" +
                         "SpeedUp Got" + ";" +
                         "SpeedDown Spawns" + ";" +
                         "SpeedDown Got" + ";" +
                         "Straight Difficulty" + ";" +
                         "Straight Speed" + ";" +
                         "Chaser Difficulty" + ";" +
                         "Chaser Speed" + ";" +
                         "Chaser HP" + ";" +
                         "Round Difficulty" + ";" +
                         "Round Bullet Speed" + ";" +
                         "Round Prepare Time" + ";" +
                         "Boomerang Difficulty" + ";" +
                         "Boomerang Bullet Speed" + ";" +
                         "Boomerang Prepare Time" + ";" +
                         "Item1" + ";" +
                         "Item2" + ";" +
                         "Item3" + ";" +
                         "Item4" + ";" +
                         "PlayerDamage" + ";" + Environment.NewLine;

        List<float> normalizedDifficulties = new List<float>();
        normalizedDifficulties.Add((balanceApplier.difficultyMultipliers[0] - balanceApplier.DifficultyMultipliersMinimum[0]) /
                                    (balanceApplier.difficultyMultipliersMaximum[0] - balanceApplier.difficultyMultipliersMinimum[0]));
        normalizedDifficulties.Add((balanceApplier.difficultyMultipliers[1] - balanceApplier.DifficultyMultipliersMinimum[1]) /
                                    (balanceApplier.difficultyMultipliersMaximum[1] - balanceApplier.difficultyMultipliersMinimum[1]));
        normalizedDifficulties.Add((balanceApplier.difficultyMultipliers[2] - balanceApplier.DifficultyMultipliersMinimum[2]) /
                                    (balanceApplier.difficultyMultipliersMaximum[2] - balanceApplier.difficultyMultipliersMinimum[2]));
        normalizedDifficulties.Add((balanceApplier.difficultyMultipliers[3] - balanceApplier.DifficultyMultipliersMinimum[3]) /
                                    (balanceApplier.difficultyMultipliersMaximum[3] - balanceApplier.difficultyMultipliersMinimum[3]));
        

        text += DateTime.Now.ToString() + ";" +
                         this.scoreManager.ElapsedTime + ";" +
                         this.scoreManager.Score + ";" +
                         this.spawnManager.EnemySpawns[0] + ";" +
                         this.scoreManager.EnemyKills[0] + ";" +
                         this.spawnManager.EnemySpawns[1] + ";" +
                         this.scoreManager.EnemyKills[1] + ";" +
                         this.spawnManager.EnemySpawns[2] + ";" +
                         this.scoreManager.EnemyKills[2] + ";" +
                         this.spawnManager.EnemySpawns[3] + ";" +
                         this.scoreManager.EnemyKills[3] + ";" +

                         this.spawnItemManager.SpawnedItems[0] + ";" +
                         this.scoreManager.ItemsGot[0] + ";" +
                         this.spawnItemManager.SpawnedItems[1] + ";" +
                         this.scoreManager.ItemsGot[1] + ";" +
                         this.spawnItemManager.SpawnedItems[2] + ";" +
                         this.scoreManager.ItemsGot[2] + ";" +
                         this.spawnItemManager.SpawnedItems[3] + ";" +
                         this.scoreManager.ItemsGot[3] + ";" +

                         normalizedDifficulties[0] + ";" +
                         this.enemyPrefabs[0].GetComponent<Characters_Enemies>().Stat_Speed * balanceApplier.difficultyMultipliers[0] + ";" +

                         normalizedDifficulties[1] + ";" +
                         this.enemyPrefabs[1].GetComponent<Characters_Enemies>().Stat_Speed * balanceApplier.difficultyMultipliers[1] + ";" +
                         this.enemyPrefabs[1].GetComponent<Characters_Global>().Stat_HP * balanceApplier.difficultyMultipliers[1] + ";" +

                         normalizedDifficulties[2] + ";" +
                         this.bulletPrefab.GetComponent<Projectiles_Global>().Speed * balanceApplier.difficultyMultipliers[2] + ";" +
                         this.enemyPrefabs[2].GetComponent<Enemies_RoundShooter>().PrepareTime * balanceApplier.difficultyMultipliers[2] + ";" +

                         normalizedDifficulties[3] + ";" +
                         this.bulletPrefab.GetComponent<Projectiles_Global>().Speed * balanceApplier.difficultyMultipliers[3] + ";" +
                         this.enemyPrefabs[3].GetComponent<Enemies_Irregular>().PrepareTime * balanceApplier.difficultyMultipliers[3] + ";" +

                         balanceApplier.itemDistances[0] + ";" +
                         balanceApplier.itemDistances[1] + ";" +
                         balanceApplier.itemDistances[2] + ";" +
                         balanceApplier.itemDistances[3] + ";" +
                         balanceApplier.DamageModifier + ";";
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

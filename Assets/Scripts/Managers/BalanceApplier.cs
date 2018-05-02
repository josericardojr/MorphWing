using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceApplier : MonoBehaviour 
{
    public static BalanceApplier instance;
    public List<float> difficultyMultipliers = new List<float>();
    float increaseMultiplier, decreaseMultiplier;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        for (int i = 0; i < 4; i++)
            difficultyMultipliers.Add(1);
    }

    public void ApplyDifficulty(int enemyID, bool increase) 
    {
        if(increase)
            Mathf.Clamp(this.difficultyMultipliers[enemyID] *= this.increaseMultiplier, 0.5f, 2);
        else
            Mathf.Clamp(this.difficultyMultipliers[enemyID] *= this.increaseMultiplier, 0.5f, 2);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceApplier : MonoBehaviour 
{
    public List<float> difficultyMultipliers = new List<float>();
    bool increase;
    float increaseMultiplier, decreaseMultiplier;

    void Awake()
    {
        for (int i = 0; i < 4; i++)
            difficultyMultipliers.Add(1);
    }

    void ApplyDifficulty(int enemyID, bool change) 
    {
        if(this.increase)
		    Mathf.Clamp(this.difficultyMultipliers[enemyID] *= this.increaseMultiplier, 0.5f, 2);
        else
            Mathf.Clamp(this.difficultyMultipliers[enemyID] *= this.increaseMultiplier, 0.5f, 2); 
	}
}

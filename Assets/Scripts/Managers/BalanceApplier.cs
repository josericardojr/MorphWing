using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceApplier : MonoBehaviour 
{
    [SerializeField]
    bool dontApplyBalance;
    public static BalanceApplier instance;
    [SerializeField]
    public List<float> difficultyMultipliers = new List<float>();
    [SerializeField]
    public List<float> difficultyMultipliersMaximum;
    [SerializeField]
    public List<float> difficultyMultipliersMinimum;
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
    }

    void Start()
    {
        if(dontApplyBalance)
            for (int i = 0; i < 4; i++)
                this.difficultyMultipliers[i] = 1;
    }

    public void ApplyDifficulty(int enemyID, bool increase) 
    {
        if(increase)
            Mathf.Clamp(this.difficultyMultipliers[enemyID] *= this.increaseMultiplier, this.difficultyMultipliersMinimum[enemyID], this.difficultyMultipliersMaximum[enemyID]);
        else
            Mathf.Clamp(this.difficultyMultipliers[enemyID] *= this.increaseMultiplier, this.difficultyMultipliersMinimum[enemyID], this.difficultyMultipliersMaximum[enemyID]);
	}
}

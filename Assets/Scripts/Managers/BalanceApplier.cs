﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceApplier : MonoBehaviour 
{
    [SerializeField]
    bool dontApplyBalance;
    public static BalanceApplier instance;
    float damageModifier;
    [SerializeField]
    public List<float> difficultyMultipliers = new List<float>();
    [SerializeField]
    public List<float> difficultyMultipliersMaximum;
    [SerializeField]
    public List<float> difficultyMultipliersMinimum;
    float increaseMultiplier, decreaseMultiplier, minValue;

    void Awake()
    {
        minValue = 1;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
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

    public void ApplyDifficulty(int enemyID, float value)
    {
        print(AcessPython.KEYENEMY[enemyID] + ": " + value);
        print("enemy" + enemyID + ": " + this.difficultyMultipliers[enemyID]);
        Mathf.Clamp(this.difficultyMultipliers[enemyID] *= value, this.difficultyMultipliersMinimum[enemyID], this.difficultyMultipliersMaximum[enemyID]);

        if (this.difficultyMultipliers[enemyID] < minValue)
        {
            this.difficultyMultipliers[enemyID] = minValue;
        }
        print("enemy" + enemyID + ": " + this.difficultyMultipliers[enemyID]);
    }

    public void ModifyDamage(float modifier)
    {
        this.damageModifier = modifier;
    }
}

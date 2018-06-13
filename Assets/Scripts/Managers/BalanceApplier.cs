using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceApplier : MonoBehaviour
{
    [SerializeField]
    bool dontApplyBalance;
    public static BalanceApplier instance;
    float damageModifier = 1;
    [SerializeField]
    public List<float> difficultyMultipliers = new List<float>();
    [SerializeField]
    public List<float> itemDistances = new List<float>();
    [SerializeField]
    public List<float> difficultyMultipliersMaximum;
    [SerializeField]
    public List<float> difficultyMultipliersMinimum;
    float increaseMultiplier, decreaseMultiplier, minValue;
    [SerializeField]
    float damageModMax, damageModMin;

    string randomID;

    public string RandomID
    {
        get { return this.randomID; }
    }

    [Header("Debug")]
    [SerializeField]
    private float[] changedDifficultyMultiplier;
    [SerializeField]
    private float[] changedItemDistances;

    [SerializeField]
    private float changedDamageModifier;

    void Awake()
    {
        this.randomID = Random.Range(0, 10).ToString() + Random.Range(0, 10).ToString() + Random.Range(0, 10).ToString() + Random.Range(0, 10).ToString() + Random.Range(0, 10).ToString() + Random.Range(0, 10).ToString();
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


        changedItemDistances = new float[4];
        for (int i = 0; i < changedItemDistances.Length; i++)
        {
            changedItemDistances[i] = (0);
        }

        changedDifficultyMultiplier = new float[4];
        for (int i = 0; i < changedDifficultyMultiplier.Length; i++)
        {
            changedDifficultyMultiplier[i] = (0);
        }

        changedDamageModifier = 0;
    }

    void Start()
    {
        if (dontApplyBalance)
        {
            for (int i = 0; i < 4; i++)
            {
                this.difficultyMultipliers[i] = 1;
            }
            this.damageModifier = 1;
        }
    }

    public void ApplyDifficulty(int enemyID, float value)
    {
        changedDifficultyMultiplier[enemyID] = this.difficultyMultipliers[enemyID];

        this.difficultyMultipliers[enemyID] = Mathf.Clamp(this.difficultyMultipliers[enemyID] * value, this.difficultyMultipliersMinimum[enemyID], this.difficultyMultipliersMaximum[enemyID]);

        if (this.difficultyMultipliers[enemyID] < minValue)
        {
            this.difficultyMultipliers[enemyID] = minValue;
        }

        changedDifficultyMultiplier[enemyID] = this.difficultyMultipliers[enemyID] - changedDifficultyMultiplier[enemyID];
    }

    public void ModifyDamage(float value)
    {
        changedDamageModifier = this.damageModifier;
        this.damageModifier = Mathf.Clamp(this.damageModifier * value, this.damageModMin, this.damageModMax);
        changedDamageModifier = this.damageModifier - changedDamageModifier;
    }

    public void SetItemDistances(int index, float value)
    {
        changedItemDistances[index] = this.itemDistances[index];
        this.itemDistances[index] = value;
        changedItemDistances[index] = this.itemDistances[index] - changedItemDistances[index];
    }

    public float[] ChangedDifficultyMultiplier
    {
        get
        {
            return changedDifficultyMultiplier;
        }
    }

    public float[] ChangedItemDistances
    {
        get
        {
            return changedItemDistances;
        }
    }

    public float ChangedDamageModifier
    {
        get
        {
            return changedDamageModifier;
        }
    }

    public float DamageModifier
    {
        get { return this.damageModifier; }
    }

    public bool DontApplyBalance
    {
        get
        {
            return dontApplyBalance;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItemManager : MonoBehaviour {

	public ItemController itemController;
    BalanceApplier balanceApplier;
	protected ExtractProvenance extractProvenance;

    [SerializeField]
    bool random;

    [SerializeField]
    Transform player;
	[SerializeField]
	List<int> effectOrder;
	[SerializeField]
	List<Transform> positionOrder;
    [SerializeField]
    List<Transform> allPositions;

	int currPowerUp;

	[SerializeField]
	private GameObject prefab;

	[SerializeField]
	private float cooldownSpawn;

	private float timeSpawn;

	Object_Efeitos.Effects auxEffect;

	[SerializeField]
	private Color colorDamageUp, colorDamageDown, colorSpeedUp, colorSpeedDown, colorInvert;
	[SerializeField]
	private Sprite spriteDamageUp, spriteDamageDown, spriteSpeedUp, spriteSpeedDown, spriteColorInvert;

	void Start ()
    {
        this.balanceApplier = GameObject.Find("Provenance").GetComponent<BalanceApplier>();
		this.transform.position = Camera.main.transform.position;
		this.extractProvenance = this.GetComponent<ExtractProvenance>();
		Prov_Agent();
        float aux = cooldownSpawn;;

        if (cooldownSpawn < aux)
        {
            cooldownSpawn = aux;
        }

		// Register the types of power ups
		foreach (Object_Efeitos.Effects eff in System.Enum.GetValues(typeof(Object_Efeitos.Effects))) {
			itemController.RegisterType(eff.ToString());
		}
	}

	void Update ()
	{
		this.timeSpawn += Time.deltaTime;
		if (this.timeSpawn >= this.cooldownSpawn)
		{
			Spawnar();
			this.timeSpawn = 0;
		}
	}

	private void Spawnar()
	{
		if(this.currPowerUp == this.effectOrder.Count)
			this.currPowerUp = 0;
        GameObject g = null;
        if (!this.random)
            g = Instantiate(this.prefab, this.positionOrder[this.currPowerUp].position, Quaternion.identity);
        else
        {
            g = Instantiate(this.prefab, new Vector2(0, 0), Quaternion.identity);
        }
        if (g != null)
        {
            g.transform.Translate(0, 0, 1);
            g.gameObject.tag = "Item";

            if (!g.GetComponent<Object_Efeitos>())
                g.AddComponent<Object_Efeitos>();

            g.GetComponent<Object_Efeitos>().SetEfeitoAtual(RandomEffect(g));
            // SPAWN ALEATÓRIO
            if (player != null && this.random)
            {
                AdjustRandomSpawnPosition(g);
                while (g.transform.position.x < -5 || g.transform.position.x > 5 ||
                    g.transform.position.y < -2.7f || g.transform.position.y > 2.7f)
                    AdjustRandomSpawnPosition(g);
            }
        }
		this.currPowerUp++;
	}

    void AdjustRandomSpawnPosition(GameObject spawnObj)
    {
        float angle = Random.Range(0.0f, Mathf.PI * 2);
        Vector3 V = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
        float distance = 0.4f;
        switch (spawnObj.GetComponent<Item>().effectId)
        {
            case 0:
                distance *= ReturnPoolValue(1, 1, 3, 2, true);
                break;
            case 1:
                distance *= ReturnPoolValue(1, 1, 3, 2, false);
                break;
            case 2:
                distance *= ReturnPoolValue(2, 3, 1, 1, true);
                break;
            case 3:
                distance *= ReturnPoolValue(2, 3, 1, 1, false);
                break;
        }
        V *= Mathf.Clamp(distance, 1, 5.1f);
        spawnObj.transform.position = player.transform.position + V;
    }

    float ReturnPoolValue(float weight1, float weight2, float weight3, float weight4, bool invertProportion)
    {
        float value;
        if(!invertProportion)
            value = (this.balanceApplier.difficultyMultipliers[0] * weight1) * (this.balanceApplier.difficultyMultipliers[1] * weight2) * (this.balanceApplier.difficultyMultipliers[2] * weight3) * (this.balanceApplier.difficultyMultipliers[3] * weight4);
        else
            value = Mathf.Clamp((3.1f - (this.balanceApplier.difficultyMultipliers[0] * weight1)), 0.1f, 3.1f) * 
                    Mathf.Clamp((3.1f - (this.balanceApplier.difficultyMultipliers[1] * weight2)), 0.1f, 3.1f) * 
                    Mathf.Clamp((3.1f - (this.balanceApplier.difficultyMultipliers[2] * weight3)), 0.1f, 3.1f) * 
                    Mathf.Clamp((3.1f - (this.balanceApplier.difficultyMultipliers[3] * weight4)), 0.1f, 3.1f);
        return value;
    }

	private Object_Efeitos.Effects RandomEffect(GameObject gameObjAtual)
	{
        int aux;
        if (!this.random)
            aux = this.effectOrder[this.currPowerUp];
        else
        {
            List<int> powerUpPool = new List<int>();

            // Damage Up
            for (int j = 0; j < 25 * ReturnPoolValue(1, 1, 3, 2, true); j++)
                powerUpPool.Add(0);
            // Damage Down
            for (int j = 0; j < 25 * ReturnPoolValue(1, 1, 3, 2, false); j++)
                powerUpPool.Add(1);
            // Speed Up
            for (int j = 0; j < 25 * ReturnPoolValue(2, 3, 1, 1, true); j++)
                powerUpPool.Add(2);
            // Damage Down
            for (int j = 0; j < 25 * ReturnPoolValue(2, 3, 1, 1, false); j++)
                powerUpPool.Add(3);

            aux = powerUpPool[Random.Range(0, powerUpPool.Count)];
        }
		switch (aux)
		{
		case 0:
			auxEffect = Object_Efeitos.Effects.DAMAGE_UP;
			gameObjAtual.GetComponent<SpriteRenderer>().color = colorDamageUp;
			gameObjAtual.GetComponent<Animator>().SetTrigger("Good");
			gameObjAtual.GetComponent<SpriteRenderer>().sprite = spriteDamageUp;
			break;
		case 1:
			auxEffect = Object_Efeitos.Effects.DAMAGE_DOWN;
			gameObjAtual.GetComponent<SpriteRenderer>().color = colorDamageDown;
			gameObjAtual.GetComponent<Animator>().SetTrigger("Bad");
			gameObjAtual.GetComponent<SpriteRenderer>().sprite = spriteDamageDown;
			break;
		case 2:
			auxEffect = Object_Efeitos.Effects.SPEED_UP;
			gameObjAtual.GetComponent<Animator>().SetTrigger("Good");
			gameObjAtual.GetComponent<SpriteRenderer>().color = colorSpeedUp;
			gameObjAtual.GetComponent<SpriteRenderer>().sprite = spriteSpeedUp;
			break;
		case 3:
			auxEffect = Object_Efeitos.Effects.SPEED_DOWN;
			gameObjAtual.GetComponent<Animator>().SetTrigger("Bad");
			gameObjAtual.GetComponent<SpriteRenderer>().color = colorSpeedDown;
			gameObjAtual.GetComponent<SpriteRenderer>().sprite = spriteSpeedDown;
			break;
		case 4:
			auxEffect = Object_Efeitos.Effects.INVERT_CONTROL;
			gameObjAtual.GetComponent<Animator>().SetTrigger("Bad");
			gameObjAtual.GetComponent<SpriteRenderer>().color = colorInvert;
			gameObjAtual.GetComponent<SpriteRenderer>().sprite = spriteColorInvert;
			break;
		}
        gameObjAtual.GetComponent<Item>().itemController = this.itemController;
        gameObjAtual.GetComponent<Item>().effectId = aux;
		itemController.Increase (auxEffect.ToString ());

		Prov_SpawnItem();
		Prov_Item(auxEffect.ToString(), gameObjAtual.GetComponent<Collider2D>().GetInstanceID().ToString());
		return auxEffect;
	}

	protected void Prov_Agent()
	{
		this.extractProvenance.NewAgentVertex("Item Spawner");
	}

	protected void Prov_SpawnItem()
	{
		int DU = itemController.GetCout(Object_Efeitos.Effects.DAMAGE_UP.ToString());
		int DD = itemController.GetCout(Object_Efeitos.Effects.DAMAGE_DOWN.ToString());
		int SU = itemController.GetCout(Object_Efeitos.Effects.SPEED_UP.ToString());
		int SD = itemController.GetCout(Object_Efeitos.Effects.SPEED_DOWN.ToString());
		int IC = itemController.GetCout(Object_Efeitos.Effects.INVERT_CONTROL.ToString());
		int HT = itemController.GetCout(Object_Efeitos.Effects.HEALTH.ToString());

		this.extractProvenance.AddAttribute ("Total_Items", "DU: " + DU + " DD: " + DD +
			" SU: " + SU + " SD: " + SD + " IC: " + IC + " HT: " + HT);
		this.extractProvenance.NewActivityVertex("Item Spawned");
	}

	protected void Prov_Item(string effect, string ID)
	{
		this.extractProvenance.AddAttribute("Effect", effect);
		this.extractProvenance.NewEntityVertex(effect);
		this.extractProvenance.GenerateInfluenceC(effect, ID, effect, "", 1);
	}

	#region GETS e SETS

	public Color ColorDamageUp
	{
		get{ return this.colorDamageUp; }
		set{ this.colorDamageUp = value; }
	}
	public Color ColorDamageDown
	{
		get { return this.colorDamageDown; }
		set { this.colorDamageDown = value; }
	}
	public Color ColorSpeedUp
	{
		get { return this.colorSpeedUp; }
		set { this.colorSpeedUp = value; }
	}
	public Color ColorSpeedDown
	{
		get { return this.colorSpeedDown; }
		set { this.colorSpeedDown = value; }
	}
	public Color ColorInvertControler
	{
		get { return this.colorInvert; }
		set { this.colorInvert = value; }
	}

	#endregion
}

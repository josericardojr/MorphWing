using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItemManager : MonoBehaviour {

	public ItemController itemController;

	protected ExtractProvenance extractProvenance;

	[SerializeField]
	List<int> effectOrder;
	[SerializeField]
	List<Transform> positionOrder;

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
		this.transform.position = Camera.main.transform.position;
		this.extractProvenance = this.GetComponent<ExtractProvenance>();
		Prov_Agent();


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
		GameObject g = Instantiate(this.prefab, this.positionOrder[this.currPowerUp].position, Quaternion.identity);
		g.transform.Translate(0,0,1);
		g.gameObject.tag = "Item";

		if (!g.GetComponent<Object_Efeitos>())
			g.AddComponent<Object_Efeitos>();

		g.GetComponent<Object_Efeitos>().SetEfeitoAtual(RandomEffect(g));
		this.currPowerUp++;
	}


	private Object_Efeitos.Effects RandomEffect(GameObject gameObjAtual)
	{
		int aux = this.effectOrder[this.currPowerUp];
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
		gameObjAtual.GetComponent<Item> ().itemController = this.itemController;
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

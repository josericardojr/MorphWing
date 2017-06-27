using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfeitoManager : MonoBehaviour {

	private bool comEfeito;

	[SerializeField]
	private float duracaoEfeitos;

	private float timeEfeito;

	//Multiplicadores dos efeitos
	[Range(1,10)]
	[SerializeField]
	private float multiDamageUp, multiDamageDown, multiSpeedUp, multiSpeedDown;

	Object_Efeitos.Effects auxEfeito;

	void Start ()
	{

	}

	void Update ()
	{
		if (this.comEfeito)
			GerenciarRetiradaEfeitos();
	}

	void GerenciarRetiradaEfeitos()
	{
		this.timeEfeito += Time.deltaTime;
		if(this.timeEfeito >= this.duracaoEfeitos)
		{
			RetirarEfeito(this.auxEfeito);
			this.timeEfeito = 0;
		}
	}

	private void ReceberEfeito(Object_Efeitos.Effects efeito)
	{
		switch (efeito)
		{
		case Object_Efeitos.Effects.DAMAGE_UP:
			GameObject[] listaPrefab;
			listaPrefab = this.GetComponent<Characters_Player>().PrefabList.ToArray();
			for (int i = 0; i < listaPrefab.Length; i++)
				listaPrefab[i].GetComponent<Projectiles_Global>().Damage =
					listaPrefab[i].GetComponent<Projectiles_Global>().Damage * multiDamageUp;

			Colorir(GameObject.Find("SpawnManager").GetComponent<SpawnItemManager>().ColorDamageUp);
			break;
		case Object_Efeitos.Effects.DAMAGE_DOWN:
			GameObject[] listaPrefab2;
			listaPrefab2 = this.GetComponent<Characters_Player>().PrefabList.ToArray();
			for (int i = 0; i < listaPrefab2.Length; i++)
				listaPrefab2[i].GetComponent<Projectiles_Global>().Damage =
					listaPrefab2[i].GetComponent<Projectiles_Global>().Damage / multiDamageDown;

			Colorir(GameObject.Find("SpawnManager").GetComponent<SpawnItemManager>().ColorDamageDown);
			break;
		case Object_Efeitos.Effects.SPEED_UP:
			this.GetComponent<Characters_Player>().SetSpeed(
				this.GetComponent<Characters_Player>().GetSpeed() * this.multiSpeedUp);

			Colorir(GameObject.Find("SpawnManager").GetComponent<SpawnItemManager>().ColorSpeedUp);
			break;
		case Object_Efeitos.Effects.SPEED_DOWN:
			this.GetComponent<Characters_Player>().SetSpeed(
				this.GetComponent<Characters_Player>().GetSpeed() / this.multiSpeedDown);

			Colorir(GameObject.Find("SpawnManager").GetComponent<SpawnItemManager>().ColorSpeedDown);
			break;
		case Object_Efeitos.Effects.INVERT_CONTROL:
			this.GetComponent<Characters_Player>().invertControl = true;

			Colorir(GameObject.Find("SpawnManager").GetComponent<SpawnItemManager>().ColorInvertControler);
			break;
		}
		this.comEfeito = true;

	}

	private void RetirarEfeito(Object_Efeitos.Effects efeito)
	{
		switch (efeito)
		{
		case Object_Efeitos.Effects.DAMAGE_UP:

			GameObject[] listaPrefab;
			listaPrefab = this.GetComponent<Characters_Player>().PrefabList.ToArray();
			for (int i = 0; i < listaPrefab.Length; i++)
				listaPrefab[i].GetComponent<Projectiles_Global>().Damage =
					listaPrefab[i].GetComponent<Projectiles_Global>().Damage / multiDamageUp;
			break;

		case Object_Efeitos.Effects.DAMAGE_DOWN:
			GameObject[] listaPrefab2;
			listaPrefab2 = this.GetComponent<Characters_Player>().PrefabList.ToArray();
			for (int i = 0; i < listaPrefab2.Length; i++)
				listaPrefab2[i].GetComponent<Projectiles_Global>().Damage =
					listaPrefab2[i].GetComponent<Projectiles_Global>().Damage * multiDamageDown;
			break;

		case Object_Efeitos.Effects.SPEED_UP:
			this.GetComponent<Characters_Player>().SetSpeed(
				this.GetComponent<Characters_Player>().GetSpeed() / this.multiSpeedUp);
			break;

		case Object_Efeitos.Effects.SPEED_DOWN:
			this.GetComponent<Characters_Player>().SetSpeed(
				this.GetComponent<Characters_Player>().GetSpeed() * this.multiSpeedDown);
			break;

		case Object_Efeitos.Effects.INVERT_CONTROL:
			this.GetComponent<Characters_Player>().invertControl = false;
			break;
		}
		Colorir(Color.white);
		this.comEfeito = false;
	}

	private void Colorir(Color color)
	{
		this.gameObject.GetComponent<SpriteRenderer>().material.color = color;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag.Equals("Item"))
		{
			if(this.comEfeito)
				RetirarEfeito(this.auxEfeito);

			this.auxEfeito = coll.gameObject.GetComponent<Object_Efeitos>().GetEfeitoAtual();

			this.GetComponent<Characters_Global>().Prov_PowerUp(auxEfeito.ToString(), coll.GetInstanceID().ToString());

			ReceberEfeito(auxEfeito);
			Destroy(coll.gameObject);
		}
	}
}

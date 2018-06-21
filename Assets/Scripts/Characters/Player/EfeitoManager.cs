using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfeitoManager : MonoBehaviour {

	private bool comEfeito;

	[SerializeField]
	private float duracaoEfeitos;

	private float timeEfeito;

    [SerializeField]
    Material speedUpMat, powerUpMat;

	//Multiplicadores dos efeitos
	[Range(0.5f,10)]
	[SerializeField]
	private float multiDamageUp, multiDamageDown, multiSpeedUp, multiSpeedDown;

	Object_Efeitos.Effects auxEfeito;

    [SerializeField]
    ScoreManager scoreManager;

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
                this.GetComponent<Animator>().SetBool("PowerBuff", true);
                this.GetComponent<TrailRenderer>().material = this.powerUpMat;
                this.GetComponent<TrailRenderer>().enabled = true;
			GameObject[] listaPrefab;
			listaPrefab = this.GetComponent<Characters_Player>().PrefabList.ToArray();
            for (int i = 0; i < listaPrefab.Length; i++)
            {
                listaPrefab[i].GetComponent<Projectiles_Global>().Damage =
                    listaPrefab[i].GetComponent<Projectiles_Global>().Damage * multiDamageUp;
                listaPrefab[i].GetComponent<Projectiles_Global>().UpEffect = true;
            }
            this.scoreManager.ItemsGot[0]++;

			break;
		case Object_Efeitos.Effects.DAMAGE_DOWN:
			GameObject[] listaPrefab2;
            this.GetComponent<Animator>().SetBool("PowerDown", true);
			listaPrefab2 = this.GetComponent<Characters_Player>().PrefabList.ToArray();
			for (int i = 0; i < listaPrefab2.Length; i++)
				listaPrefab2[i].GetComponent<Projectiles_Global>().Damage =
					listaPrefab2[i].GetComponent<Projectiles_Global>().Damage / multiDamageDown;
            this.scoreManager.ItemsGot[1]++;

			break;
        case Object_Efeitos.Effects.SPEED_UP:
            this.GetComponent<Characters_Player>().speedMultiplier = this.multiSpeedUp;
            this.GetComponent<TrailRenderer>().material = this.speedUpMat;
            this.GetComponent<Animator>().SetBool("SpeedBuff", true);
            this.GetComponent<TrailRenderer>().enabled = true;
            this.scoreManager.ItemsGot[2]++;

			break;
        case Object_Efeitos.Effects.SPEED_DOWN:
            this.GetComponent<Animator>().SetBool("SpeedDown", true);
            this.GetComponent<Characters_Player>().speedMultiplier = this.multiSpeedDown;
            this.scoreManager.ItemsGot[3]++;
			break;
		case Object_Efeitos.Effects.INVERT_CONTROL:
			this.GetComponent<Characters_Player>().invertControl = true;
            this.GetComponent<Animator>().SetBool("SpeedDown", true);

			break;
		}
		this.comEfeito = true;

	}

	private void RetirarEfeito(Object_Efeitos.Effects efeito)
	{
		switch (efeito)
		{
		case Object_Efeitos.Effects.DAMAGE_UP:

            this.GetComponent<Animator>().SetBool("PowerBuff", false);
            this.GetComponent<TrailRenderer>().enabled = false;
			GameObject[] listaPrefab;
			listaPrefab = this.GetComponent<Characters_Player>().PrefabList.ToArray();
            for (int i = 0; i < listaPrefab.Length; i++)
            {
                listaPrefab[i].GetComponent<Projectiles_Global>().Damage =
                    listaPrefab[i].GetComponent<Projectiles_Global>().Damage / multiDamageUp;
                listaPrefab[i].GetComponent<Projectiles_Global>().UpEffect = false;
            }
			break;

		case Object_Efeitos.Effects.DAMAGE_DOWN:
			GameObject[] listaPrefab2;
            this.GetComponent<Animator>().SetBool("PowerDown", false);
			listaPrefab2 = this.GetComponent<Characters_Player>().PrefabList.ToArray();
			for (int i = 0; i < listaPrefab2.Length; i++)
				listaPrefab2[i].GetComponent<Projectiles_Global>().Damage =
					listaPrefab2[i].GetComponent<Projectiles_Global>().Damage * multiDamageDown;
			break;

        case Object_Efeitos.Effects.SPEED_UP:
            this.GetComponent<Characters_Player>().speedMultiplier = 1;
            this.GetComponent<Animator>().SetBool("SpeedBuff", false);
            this.GetComponent<TrailRenderer>().enabled = false;
			break;

        case Object_Efeitos.Effects.SPEED_DOWN:
            this.GetComponent<Animator>().SetBool("SpeedDown", false);
            this.GetComponent<Characters_Player>().speedMultiplier = 1;
			break;

		case Object_Efeitos.Effects.INVERT_CONTROL:
			this.GetComponent<Characters_Player>().invertControl = false;
            this.GetComponent<Animator>().SetBool("SpeedDown", false);
			break;
		}
		this.comEfeito = false;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag.Equals("Item"))
		{
			if(this.comEfeito)
				RetirarEfeito(this.auxEfeito);

			this.auxEfeito = coll.gameObject.GetComponent<Object_Efeitos>().GetEfeitoAtual();
			Item i = coll.GetComponent<Item> ();
			i.enabled = false;
			i.itemController.Decrease (i.effect.ToString());

			this.GetComponent<Characters_Global>().Prov_PowerUp(auxEfeito.ToString(), coll.GetInstanceID().ToString());

			ReceberEfeito(auxEfeito);
			Destroy(coll.gameObject);
		}
	}
}

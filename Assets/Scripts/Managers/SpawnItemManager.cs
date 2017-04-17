using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItemManager : MonoBehaviour {

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private float cooldownSpawn;

    private float timeSpawn;

    Object_Efeitos.Effects auxEffect;

    [SerializeField]
    private Color colorDamageUp, colorDamageDown, colorSpeedUp, colorSpeedDown, colorInvert;

    void Start ()
    {
        this.transform.position = Camera.main.transform.position;
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
       GameObject g = Instantiate(this.prefab, RandomScreenPosition(), Quaternion.identity);
        g.gameObject.tag = "Item";

        if (!g.GetComponent<Object_Efeitos>())
            g.AddComponent<Object_Efeitos>();

        g.GetComponent<Object_Efeitos>().SetEfeitoAtual(RandomEffect(g));
    }

    private Vector3 RandomScreenPosition()
    {
        float x = Random.Range((float)(-(Screen.width / 100)), (float)Screen.width / 100);
        float y = Random.Range((float)-(Screen.height / 100), (float)Screen.height / 100);
        return new Vector3(x, y, 1);
    }

    private Object_Efeitos.Effects RandomEffect(GameObject gameObjAtual)
    {
        short aux = (short)Random.Range(0, 5);
        switch (aux)
        {
            case 0:
                auxEffect = Object_Efeitos.Effects.DAMAGE_UP;
                gameObjAtual.GetComponent<SpriteRenderer>().color = colorDamageUp;
                break;
            case 1:
                auxEffect = Object_Efeitos.Effects.DAMAGE_DOWN;
                gameObjAtual.GetComponent<SpriteRenderer>().color = colorDamageDown;
                break;
            case 2:
                auxEffect = Object_Efeitos.Effects.SPEED_UP;
                gameObjAtual.GetComponent<SpriteRenderer>().color = colorSpeedUp;
                break;
            case 3:
                auxEffect = Object_Efeitos.Effects.SPEED_DOWN;
                gameObjAtual.GetComponent<SpriteRenderer>().color = colorSpeedDown;
                break;
            case 4:
                auxEffect = Object_Efeitos.Effects.INVERT_CONTROL;
                gameObjAtual.GetComponent<SpriteRenderer>().color = colorInvert;
                break;
        }

        return auxEffect;
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

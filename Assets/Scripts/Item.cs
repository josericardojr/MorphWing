using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	public float lifeTime = 4;
	public Object_Efeitos.Effects effect;
	public ItemController itemController;

	// Use this for initialization
	void Start ()
	{
		Invoke("SetCatchable", 1);
	}

	void SetCatchable()
	{
		this.GetComponent<SpriteRenderer>().color = new Color(this.GetComponent<SpriteRenderer>().color.r, this.GetComponent<SpriteRenderer>().color.g, this.GetComponent<SpriteRenderer>().color.b, 255);
		this.GetComponent<BoxCollider2D>().enabled = true;
	}

	// Update is called once per frame
	void Update () 
    {
		lifeTime -= Time.deltaTime;

		if (lifeTime <= 0) {
			itemController.Decrease (effect.ToString ());
			Destroy (this.gameObject);
		}
	}
}

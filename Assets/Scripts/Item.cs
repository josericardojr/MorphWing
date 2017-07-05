using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	public float lifeTime = 4;
	public Object_Efeitos.Effects effect;
	public ItemController itemController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		lifeTime -= Time.deltaTime;

		if (lifeTime <= 0) {
			itemController.Decrease (effect.ToString ());
			Destroy (this.gameObject);
		}
	}
}

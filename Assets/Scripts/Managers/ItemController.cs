using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {

	Dictionary<string, int> items = new Dictionary<string, int>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RegisterType(string type){
		items.Add (type, 0);
	}

	public void Increase(string type){
		if (items.ContainsKey(type)){
			items [type] = items [type] + 1;
		}
	}

	public void Decrease(string type){
		if (items.ContainsKey (type)) {
			items [type] = items [type] - 1;
		}
	}

	public int GetCout(string type){
		if (items.ContainsKey (type)) {
			return items [type];
		} else
			return -100;
	}
}

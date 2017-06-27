using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects_Health : MonoBehaviour
{

	void OnTriggerEnter2D(Collider2D c)
	{
		if(c.transform.tag.Equals("Player"))
		{
			c.GetComponent<Characters_Player>().Heal(this.GetComponent<Collider2D>().GetInstanceID().ToString());
			Destroy(this.gameObject);
		}
	}
}

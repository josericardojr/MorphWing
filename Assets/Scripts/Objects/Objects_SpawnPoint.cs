using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects_SpawnPoint : MonoBehaviour
{
	[SerializeField]
	Vector2 initDir;

	public Vector2 InitDir
	{
		get{return this.initDir;}
	}
}

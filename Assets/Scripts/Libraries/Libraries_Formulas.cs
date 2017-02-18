using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Libraries_Formulas
{
	public static int ReturnDirection(float pos1, float pos2)
	{
		if(pos1 > pos2)
			return -1;
		else
			return 1;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomEx
{
	public static Vector3 RangeVector3(Vector3 min,Vector3 max)
	{
		var res = new Vector3();
		res.x = Random.Range(min.x, max.x);
		res.y = Random.Range(min.y, max.y);
		res.z = Random.Range(min.z, max.z);
		return res;
	}

	public static Vector2 RangeVector2(Vector2 min,Vector2 max)
	{
		var res = new Vector2();
		res.x = Random.Range(min.x, max.x);
		res.y = Random.Range(min.y, max.y);
		return res;
	}
}
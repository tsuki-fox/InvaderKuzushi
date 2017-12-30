using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Ex
{
	public static Vector3 ToVector3(this Vector2 self)
	{
		return new Vector3(self.x, self.y, 0);
	}
	public static Vector3 ToVector3(this Vector2 self,float z)
	{
		return new Vector3(self.x, self.y, z);
	}

	public static float ToAngle(this Vector2 self)
	{
		return Mathf.Atan2(self.y, self.x) * Mathf.Rad2Deg;
	}
}
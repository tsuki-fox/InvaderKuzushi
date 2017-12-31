using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Ex
{
	public static Vector2 ToVector2(this Vector3 self)
	{
		return new Vector2(self.x, self.y);
	}
}
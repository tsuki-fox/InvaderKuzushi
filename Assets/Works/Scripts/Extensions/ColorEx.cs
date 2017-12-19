using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorEx
{
	public static void SetRed(this Color self,float r)
	{
		self = new Color(r, self.g, self.b, self.a);
	}

	public static void SetGreen(this Color self,float g)
	{
		self = new Color(self.r, g, self.b, self.a);
	}

	public static void SetBlue(this Color self,float b)
	{
		self = new Color(self.r, self.g, b, self.a);
	}

	public static void SetAlpha(this Color self,float a)
	{
		self = new Color(self.r, self.g, self.b, a);
	}
}

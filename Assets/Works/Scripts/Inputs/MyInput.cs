using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyInput
{
	public static bool isLeft
	{
		get { return Input.GetKey(KeyCode.LeftArrow); }
	}

	public static bool isRight
	{
		get { return Input.GetKey(KeyCode.RightArrow); }
	}

	public static bool downShot
	{
		get { return Input.GetKeyDown(KeyCode.Space); }
	}
}
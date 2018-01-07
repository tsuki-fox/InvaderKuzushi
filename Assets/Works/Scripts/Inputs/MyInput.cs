using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyInput
{
	//! ----internal----
	static bool _locked = false;

	//! ----properties----
	public static bool locked
	{
		get { return _locked; }
		set { _locked = value; }
	}

	public static bool left
	{
		get { return Input.GetKey(KeyCode.LeftArrow) && !locked; }
	}
	public static bool right
	{
		get { return Input.GetKey(KeyCode.RightArrow) && !locked; }
	}
	public static bool up
	{
		get { return Input.GetKey(KeyCode.UpArrow) && !locked; }
	}
	public static bool down
	{
		get { return Input.GetKey(KeyCode.DownArrow) && !locked; }
	}

	public static bool downShot
	{
		get { return Input.GetKeyDown(KeyCode.Space) && !locked; }
	}

	public static bool missileShot
	{
		get { return Input.GetKeyDown(KeyCode.W) && !locked; }
	}
	
	public static bool ok
	{
		get { return (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && !locked; }
	}

	public static bool cursorUp
	{
		get { return Input.GetKeyDown(KeyCode.UpArrow) && !locked; }
	}
	public static bool cursorDown
	{
		get { return Input.GetKeyDown(KeyCode.DownArrow) && !locked; }
	}

	public static bool anyKey
	{
		get { return Input.anyKey && !locked; }
	}
	public static bool anyKeyDown
	{
		get { return Input.anyKeyDown && !locked; }
	}
}
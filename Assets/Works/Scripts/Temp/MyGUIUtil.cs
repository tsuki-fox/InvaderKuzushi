using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyGUIUtil
{
	static Color _guiColor;
	static Color _guiContentColor;
	static Color _guiBackgroundColor;

	public static void BeginColorChange(Color color)
	{
		_guiColor = GUI.color;
		GUI.color = color;
	}
	public static void EndColorChange()
	{
		GUI.color = _guiColor;
	}

	public static void BeginContentColorChange(Color color)
	{
		_guiContentColor = GUI.contentColor;
		GUI.contentColor = color;
	}
	public static void EndContentColorChange()
	{
		GUI.contentColor = _guiContentColor;
	}

	public static void BeginBackgroundColorChange(Color color)
	{
		_guiBackgroundColor = GUI.backgroundColor;
		GUI.backgroundColor = color;
	}
	public static void EndBackgroundColorChange()
	{
		GUI.backgroundColor = _guiBackgroundColor;
	}
}
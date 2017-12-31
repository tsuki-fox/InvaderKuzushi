using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameActivityBus
{
	//! --------events--------
	public delegate void OnGameOverHandler();
	public static OnGameOverHandler onGameOver = delegate { };

	//! --------functions--------
	public static void NotifyGameOver()
	{
		onGameOver();
	}

	public static void Clean()
	{
		onGameOver = delegate { };
	}
}

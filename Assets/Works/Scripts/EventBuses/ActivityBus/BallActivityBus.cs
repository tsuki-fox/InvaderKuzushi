using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BallActivityBus
{
	//! --------event declares--------
	public delegate void OnDropHandler(GameObject ball);
	public static OnDropHandler onDrop = delegate { };

	//! --------functions--------
	public static void NotifyDrop(GameObject ball)
	{
		onDrop(ball);
	}

	public static void Clean()
	{
		onDrop = delegate { };
	}
}

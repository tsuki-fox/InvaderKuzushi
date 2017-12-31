using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerActivityBus
{
	//! --------type declares--------
	public enum Direction
	{
		Neutral,
		Left,
		Right,
		Both,
	}

	public delegate void OnShotHandler(GameObject player);
	public delegate void OnMoveHandler(GameObject player, Direction direction);
	//! --------value declares--------
	public static event OnShotHandler onShot = delegate { };
	public static event OnMoveHandler onMove = delegate { };

	//! --------functions--------
	public static void NotifyShot(GameObject player)
	{
		onShot(player);
	}

	public static void NotifyMove(GameObject player, Direction direction)
	{
		onMove(player, direction);
	}

	public static void Clean()
	{
		onShot = delegate { };
		onMove = delegate { };
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActivityNotifier : MonoBehaviour
{
	//! --------functions--------
	void NotifyShot()
	{
		if (MyInput.downShot)
			PlayerActivityBus.NotifyShot(gameObject);
	}

	void NotifyMove()
	{
		bool left = MyInput.left;
		bool right = MyInput.right;

		if (left && right)
			PlayerActivityBus.NotifyMove(gameObject, PlayerActivityBus.Direction.Both);
		else if (left)
			PlayerActivityBus.NotifyMove(gameObject, PlayerActivityBus.Direction.Left);
		else if (right)
			PlayerActivityBus.NotifyMove(gameObject, PlayerActivityBus.Direction.Right);
		else
			PlayerActivityBus.NotifyMove(gameObject, PlayerActivityBus.Direction.Neutral);
	}

	void Update()
	{
		NotifyShot();
		NotifyMove();
	}
}

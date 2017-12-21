using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TF;

public class PlayerActivityRuler : MonoBehaviour
{
	//! --------value declares--------
	[SerializeField]
	float _moveSpeed;
	[SerializeField]
	GameObject _sourceBullet;
	[SerializeField]
	float _shotVelocity;

	//! --------functions--------
	void Awake()
	{
		PlayerActivityBus.onMove += PlayerActivityBus_onMove;
		PlayerActivityBus.onShot += PlayerActivityBus_onShot;
	}

	private void PlayerActivityBus_onShot(GameObject player)
	{
		var bullet = ObjectPool.Alloc(_sourceBullet);
		bullet.transform.position = player.transform.position;
		bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, _shotVelocity);
	}

	void PlayerActivityBus_onMove(GameObject player, PlayerActivityBus.Direction direction)
	{
		var rigidbody = player.GetComponent<Rigidbody2D>();

		switch (direction)
		{
			case PlayerActivityBus.Direction.Neutral:
				rigidbody.velocity = Vector2.zero;
				break;
			case PlayerActivityBus.Direction.Left:
				rigidbody.velocity = new Vector2(-_moveSpeed, 0f);
				break;
			case PlayerActivityBus.Direction.Right:
				rigidbody.velocity = new Vector2(_moveSpeed, 0f);
				break;
			case PlayerActivityBus.Direction.Both:
				rigidbody.velocity = Vector2.zero;
				break;
		}
	}

	void OnDestroy()
	{
		PlayerActivityBus.Clean();
	}
}

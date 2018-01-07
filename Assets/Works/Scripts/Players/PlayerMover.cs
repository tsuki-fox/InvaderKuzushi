using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Players
{
	public class PlayerMover : BasePlayerComponent
	{
		//! --------parameters--------
		[SerializeField,Header("移動速度")]
		float _moveSpeed = 3f;

		//! --------functions--------
		void Move()
		{
			Vector2 velocity = new Vector2();
			if (MyInput.left)
				velocity.x = -_moveSpeed;
			if (MyInput.right)
				velocity.x = _moveSpeed;
			rigidbody2D.velocity = velocity;
		}

		//! --------unity functions--------
		void Update()
		{
			Move();
		}
	}
}
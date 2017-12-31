using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Balls
{
	public abstract class BaseBallComponent : MonoBehaviour
	{
		//! ----internal variables----
		Rigidbody2D _rigidbody2d;
		BallCore _core;
		BallCollider _collider;

		//! ----properties----
		protected new Rigidbody2D rigidbody2D { get { return _rigidbody2d; } }
		protected BallCore core { get { return _core; } }
		protected new BallCollider collider { get { return _collider; } }

		//! ----life cycles----
		void Awake()
		{
			_rigidbody2d = GetComponent<Rigidbody2D>();
			_core = GetComponent<BallCore>();
			_collider = GetComponent<BallCollider>();
		}
	}
}
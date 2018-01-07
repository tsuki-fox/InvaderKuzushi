using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Balls
{
	public abstract class BaseBallComponent : Cores.BaseComponent
	{
		//! ----internal variables----
		Rigidbody2D _rigidbody2d;
		BallCore _core;

		//! ----properties----
		protected new Rigidbody2D rigidbody2D { get { return _rigidbody2d; } }
		protected BallCore core { get { return _core; } }

		public override void Prepare()
		{
			base.Prepare();
			_rigidbody2d = GetComponent<Rigidbody2D>();
			_core = GetComponent<BallCore>();
		}
	}
}
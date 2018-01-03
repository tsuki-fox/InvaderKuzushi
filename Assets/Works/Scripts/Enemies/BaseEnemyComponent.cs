using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Cores;

namespace Assets.Enemies
{
	public abstract class BaseEnemyComponent : BaseComponent
	{
		//! ----internal variables----
		Rigidbody2D _rigidbody2D;
		EnemyCore _core;

		//! ----properties----
		protected new Rigidbody2D rigidbody2D { get { return _rigidbody2D; } }
		protected EnemyCore core { get { return _core; } }

		//! ----overrides----
		public override void Prepare()
		{
			base.Prepare();
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_core = GetComponent<EnemyCore>();
		}
	}
}
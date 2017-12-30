using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Enemies
{
	public abstract class BaseEnemyComponent : MonoBehaviour
	{
		//! ----internal variables----
		Rigidbody2D _rigidbody2D;
		EnemyCore _core;

		//! ----properties----
		protected new Rigidbody2D rigidbody2D { get { return _rigidbody2D; } }
		protected EnemyCore core { get { return _core; } }

		//! ----life cycles----
		void Awake()
		{
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_core = GetComponent<EnemyCore>();
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Cores;

namespace Assets.Players
{
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(PlayerCore))]
	public abstract class BasePlayerComponent : BaseComponent
	{
		//! --------internal variables--------
		Rigidbody2D _rigidbody2d;
		PlayerCore _core;
		PlayerMover _mover;
		PlayerMissileController _missileController;
		PlayerFireController _fireController;

		//! --------properties--------
		protected new Rigidbody2D rigidbody2D { get { return _rigidbody2d; } }
		protected PlayerCore core { get { return _core; } }
		protected PlayerMover mover { get { return _mover; } }
		protected PlayerMissileController missileController { get { return _missileController; } }
		protected PlayerFireController fireController { get { return _fireController; } }

		//! --------life cycles--------
		void Awake()
		{
			_rigidbody2d = GetComponent<Rigidbody2D>();
			_core = GetComponent<PlayerCore>();
			_missileController = GetComponent<PlayerMissileController>();
			_mover = GetComponent<PlayerMover>();
			_fireController = GetComponent<PlayerFireController>();
		}
	}
}
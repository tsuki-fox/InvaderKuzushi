using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Missiles
{
	public class BaseMissileComponent : Cores.BaseComponent
	{
		//! ----internal variables----
		Rigidbody2D _rigidbody2D;
		MissileCore _core;
		MissileSeeker _seeker;

		//! ----properties----
		public new Rigidbody2D rigidbody2D { get { return _rigidbody2D; } }
		public MissileCore core { get { return _core; } }
		public MissileSeeker seeker { get { return _seeker; } }

		//! ----overrides----
		public override void Prepare()
		{
			base.Prepare();
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_core = GetComponent<MissileCore>();
			_seeker = GetComponent<MissileSeeker>();
		}
	}
}
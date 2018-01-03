using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Cores;
using Assets.Damages;

namespace Assets.Missiles
{
	public class MissileCore : BaseCore, Damages.IKillable
	{
		//! ----events----
		public event OnKilledHandler onKilled;

		//! ----parameters----
		float _damage = 10f;

		//! ----properties----
		public float damage { get { return _damage; } }

		//! ----functions----
		public void Kill()
		{
			onKilled();
			TF.ObjectPool.Free(gameObject);
		}

		//! ----overrides----
		public override void Clean()
		{
			base.Clean();
			onKilled = delegate { };
		}
	}
}
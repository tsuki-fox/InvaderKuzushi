using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Damages;
using Assets.Core;
using UniRx;

namespace Assets.Balls
{
	public class BallCore : Cores.BaseCore, IKillable
	{
		//! ----events----
		public event OnKilledHandler onKilled = delegate { };

		//! ----parameters----
		[SerializeField]
		float _damage;

		//! ----properties----
		public float damage { get { return _damage; } }

		//! ----functions----
		public override void Clean()
		{
			base.Clean();
			onKilled = delegate { };
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public void Kill()
		{
			onKilled();
			TF.ObjectPool.Free(gameObject);
		}
	}
}
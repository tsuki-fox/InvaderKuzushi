using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Cores;
using Assets.Damages;
using TF;

namespace Assets.PlayerBullets
{
	public class PlayerBulletCore : BaseCore, IKillable
	{
		//! ----parameters----
		[SerializeField]
		float _damage;

		//! ----properties----
		public float damage { get { return _damage; } }

		//! ----events----
		public event OnKilledHandler onKilled = delegate { };

		//! ----functions----
		public override void Clean()
		{
			onKilled = delegate { };
		}

		public void Kill()
		{
			if(onKilled!=null)
				onKilled();
			ObjectPool.Free(gameObject);
		}

		public override void Initialize()
		{
			base.Initialize();
			Debug.Log("bullet initialized");
		}
	}
}
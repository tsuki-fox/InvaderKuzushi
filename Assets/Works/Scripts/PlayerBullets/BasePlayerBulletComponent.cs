using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Cores;

namespace Assets.PlayerBullets
{
	public abstract class BasePlayerBulletComponent : BaseComponent
	{
		//! ----internal variables----
		PlayerBulletCore _core;
		PlayerBulletAttacker _attacker;

		//! ----properties----
		protected PlayerBulletCore core { get { return _core; } }
		protected PlayerBulletAttacker attacker { get { return _attacker; } }

		public override void Prepare()
		{
			_core = GetComponent<PlayerBulletCore>();
			_attacker = GetComponent<PlayerBulletAttacker>();
		}
	}
}
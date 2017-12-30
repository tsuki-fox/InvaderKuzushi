using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.PlayerBullets
{
	public abstract class BasePlayerBulletComponent : MonoBehaviour
	{
		//! ----internal variables----
		PlayerBulletCore _core;
		PlayerBulletAttacker _attacker;

		//! ----properties----
		protected PlayerBulletCore core { get { return _core; } }
		protected PlayerBulletAttacker attacker { get { return _attacker; } }

		//! ----life cycles----
		void Awake()
		{
			_core = GetComponent<PlayerBulletCore>();
			_attacker = GetComponent<PlayerBulletAttacker>();
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Damages;
using Assets.Cores;
using UniRx;
using TF;

namespace Assets.Enemies
{
	public class EnemyCore : BaseCore,IDamageable
	{
		//! ----parameters----
		[SerializeField]
		float _maxHP = 10;

		//! ----internal variables----
		float _hp;

		//! ----delegate declares----

		//! ----events----
		public event OnDamagedHandler onDamaged = delegate { };
		public event OnKilledHandler onKilled = delegate { };
		public event OnDeadHandler onDead = delegate { };

		//! ----properties----
		public float maxHP { get { return _maxHP; } }
		public float hp { get { return _hp; } }

		//! ----functions----
		public void ApplyDamage(float damageTaken)
		{
			_hp -= damageTaken;
			onDamaged(damageTaken);
			if (_hp <= 0)
			{
				onDead();
				ObjectPool.Free(gameObject);
			}
		}

		public void Kill()
		{
			onKilled();
		}

		public override void Clean()
		{
			base.Clean();
			onDamaged = delegate { };
			onKilled = delegate { };
			onDead = delegate { };
		}

		public override void Initialize()
		{
			base.Initialize();
			_hp = _maxHP;
		}
	}
}
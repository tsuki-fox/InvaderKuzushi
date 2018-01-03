using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Assets.Damages;
using Assets.Cores;

namespace Assets.Players
{
	public class PlayerCore : BaseCore, IDamageable
	{
		//! --------parameters--------
		[SerializeField]
		float _maxHP = 100;

		//! --------internal variables--------
		[Extractable]
		float _hp;

		//! --------delegate declares--------

		//! --------events--------
		public event OnDamagedHandler onDamaged = delegate { };
		public event OnDeadHandler onDead = delegate { };

		//! --------properties--------
		public float maxHP { get { return _maxHP; } }
		public float hp { get { return _hp; } }

		//! --------functions--------
		public override void Prepare()
		{
		}

		public override void Clean()
		{
			onDamaged = delegate { };
			onDead = delegate { };
		}

		public override void Initialize()
		{
			_hp = _maxHP;
		}

		void IDamageable.ApplyDamage(float damageTaken)
		{
			_hp -= damageTaken;
			onDamaged(damageTaken);

			if (_hp <= 0)
				onDead();
		}
	}
}
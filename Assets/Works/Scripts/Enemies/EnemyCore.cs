using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Damages;
using Assets.Core;
using UniRx;
using TF;

namespace Assets.Enemies
{
	public class EnemyCore : MonoBehaviour, IDamageable, IKillable, ICore
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
		public event OnCleanedHandler onCleaned = delegate { };
		public event OnInitializedHandler onInitialized = delegate { };
		public event OnReleasedHandler onReleased = delegate { };

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

		public void Clean()
		{
			onDamaged = delegate { };
			onKilled = delegate { };
			onDead = delegate { };
			onCleaned();
		}

		public void Initialize()
		{
			Clean();
			_hp = _maxHP;
			onInitialized();
		}

		//! ----life cycles----
		void Start()
		{
			Observable.NextFrame().Subscribe(_ =>
			{
				Initialize();
			});
		}

		//! ----object pool support----
		void OnAlloc()
		{
			Initialize();
		}

		void OnFree()
		{
			onReleased();
		}
	}
}
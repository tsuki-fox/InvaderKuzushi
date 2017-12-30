using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Assets.Damages;
using Assets.Core;

namespace Assets.Players
{
	public class PlayerCore : MonoBehaviour, IDamageable, ICore
	{
		//! --------parameters--------
		[SerializeField]
		float _maxHP = 100;

		//! --------internal variables--------
		[Extractable]
		float _hp;

		//! --------delegate declares--------

		//! --------events--------
		public event OnCleanedHandler onCleaned = delegate { };
		public event OnInitializedHandler onInitialized = delegate { };
		public event OnDamagedHandler onDamaged = delegate { };
		public event OnDeadHandler onDead = delegate { };
		public event OnReleasedHandler onReleased = delegate { };

		//! --------properties--------
		public float maxHP { get { return _maxHP; } }
		public float hp { get { return _hp; } }

		//! --------functions--------
		public void Clean()
		{
			onDamaged = delegate { };
			onDead = delegate { };
			onCleaned();
		}

		public void Initialize()
		{
			Clean();
			_hp = _maxHP;
			onInitialized();
		}

		void IDamageable.ApplyDamage(float damageTaken)
		{
			_hp -= damageTaken;
			onDamaged(damageTaken);

			if (_hp <= 0)
				onDead();
		}

		//! --------life cycles--------
		void Start()
		{
			Observable.NextFrame().Subscribe(_ =>
			{
				Initialize();
			});
		}

		//! --------object pool support--------
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Assets.Damages;
using Assets.Cores;
using DG.Tweening;

namespace Assets.Players
{
	public class PlayerCore : BaseCore, IDamageable
	{
		//! ----types----
		[System.Serializable]
		public class CameraShakeParams
		{
			public float duration = 1f;
			public float strength = 3f;
			public int vibrato = 10;
			public float randomness = 90f;
		}

		//! --------parameters--------
		[SerializeField]
		float _maxHP = 100;
		[SerializeField]
		float _fallenDamage = 25;
		[SerializeField]
		CameraShakeParams _cameraShakeParams;
		//! --------internal variables--------
		[Extractable]
		float _hp;

		Collisions.CollisionBus.Unsubscriber _fallenProc;
		static PlayerCore _instance = null;

		//! --------delegate declares--------

		//! --------events--------
		public event OnDamagedHandler onDamaged = delegate { };
		public event OnDeadHandler onDead = delegate { };

		//! --------properties--------
		public float maxHP { get { return _maxHP; } }
		public float hp { get { return _hp; } }
		public static PlayerCore instance { get { return _instance; } }

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
			_fallenProc = Collisions.CollisionBus.Subscribe(TagName.Ball, TagName.LaserFence,
			(ball, fence, collision) =>
			{
				ApplyDamage(_fallenDamage);
			});
			logger.Log("Initialized from :{0}", StackInfos.GetCallerTrace(2));
			_instance = FindObjectOfType<PlayerCore>();
		}

		public override void Release()
		{
			base.Release();
			_fallenProc.Unsubscribe();
		}

		public void ApplyDamage(float damageTaken)
		{
			_hp -= damageTaken;
			onDamaged(damageTaken);

			Camera.main.DOComplete();
			Camera.main.DOShakePosition(
				_cameraShakeParams.duration,
				_cameraShakeParams.strength,
				_cameraShakeParams.vibrato,
				_cameraShakeParams.randomness);

			if (_hp <= 0)
			{
				onDead();
				TF.ObjectPool.Free(gameObject);
			}
		}
	}
}
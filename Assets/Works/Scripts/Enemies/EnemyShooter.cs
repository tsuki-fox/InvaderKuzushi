using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TF;
using UniRx;

namespace Assets.Enemies
{
	public class EnemyShooter : BaseEnemyComponent
	{
		//! ----parameters----
		[SerializeField]
		float _interval;
		[SerializeField]
		GameObject _bulletPrefab;
		[SerializeField]
		float _shotVelocity;

		//! ----internal----
		IDisposable _disposer;

		//! ----functions----
		void Shot()
		{
			var bullet = ObjectPool.Alloc(_bulletPrefab);
			bullet.transform.position = transform.position;

			bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -_shotVelocity);
		}

		//! ----overrides----
		public override void Initialize()
		{
			base.Initialize();

			_disposer = Caller.Interval(_interval).Subscribe(_ => Shot());
		}
		public override void Release()
		{
			base.Release();
			_disposer.Dispose();
		}
	}
}
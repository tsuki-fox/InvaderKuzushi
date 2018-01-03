using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Assets.Missiles
{
	public class MissileEffectEmitter : BaseMissileComponent
	{
		//! ----parameters----
		[SerializeField]
		GameObject _lockOnMarkerVFX;
		[SerializeField]
		GameObject _smokeVFX;
		[SerializeField]
		float _smokeInterval = 0.1f;

		//! ----internal----
		System.IDisposable _disposer;

		//! ----overrides----
		public override void Initialize()
		{
			base.Initialize();
			seeker.onLocked += (enemy) =>
			{
				var vfx = TF.ObjectPool.Alloc(_lockOnMarkerVFX, 1f);
				vfx.transform.position = enemy.transform.position;
				Observable.Interval(System.TimeSpan.FromSeconds(0.016f)).Subscribe(_ =>
				{
					vfx.transform.position = enemy.transform.position;
				}).Delay(1f).Dispose();
			};

			Debug.Log("Initialize");
			_disposer = Observable.Interval(System.TimeSpan.FromSeconds(_smokeInterval)).Subscribe(_ =>
			{
				var vfx = TF.ObjectPool.Alloc(_smokeVFX, 1f);
				vfx.transform.position = transform.position;
			});

			var caller = StackInfos.GetCallerFullName();
			Debug.LogFormat("caller:{0}", caller);
		}

		public override void Release()
		{
			Debug.Log("dispose");
			base.Release();
			_disposer.Dispose();
		}
	}
}
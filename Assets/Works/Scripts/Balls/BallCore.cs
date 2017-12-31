using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Damages;
using Assets.Core;
using UniRx;

namespace Assets.Balls
{
	public class BallCore : MonoBehaviour, ICore, IKillable
	{
		//! ----events----
		public event OnCleanedHandler onCleaned;
		public event OnInitializedHandler onInitialized;
		public event OnReleasedHandler onReleased;
		public event OnKilledHandler onKilled;

		//! ----functions----
		public void Clean()
		{
			onKilled = delegate { };
			onCleaned();
		}

		public void Initialize()
		{
			Clean();
			onInitialized();
		}

		public void Kill()
		{
			onKilled();
			TF.ObjectPool.Free(gameObject);
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
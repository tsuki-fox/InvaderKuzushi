using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Core;
using Assets.Damages;
using TF;

namespace Assets.PlayerBullets
{
	public class PlayerBulletCore : MonoBehaviour, ICore, IKillable
	{
		//! ----parameters----
		//! ----events----
		public event OnCleanedHandler onCleaned = delegate { };
		public event OnInitializedHandler onInitialized = delegate { };
		public event OnReleasedHandler onReleased = delegate { };
		public event OnKilledHandler onKilled = delegate { };

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
			ObjectPool.Free(gameObject);
		}

		//! ----object pool----
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
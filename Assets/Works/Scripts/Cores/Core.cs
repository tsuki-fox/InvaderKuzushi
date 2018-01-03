using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Collisions;
using UniRx;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Cores
{
	[RequireComponent(typeof(Assets.Cores.Logger))]
	public abstract class BaseCore : MonoBehaviour
	{
		//! ----delegates----
		public delegate void OnInitializedHandler();
		public delegate void OnReleasedHandler();

		//! ----events----
		public event OnInitializedHandler onInitialized = delegate { };
		public event OnReleasedHandler onReleased = delegate { };

		//! ----internal values----
		Logger _logger;

		//! ----properties----
		protected Logger logger { get { return _logger; } }

		//! ----collisions----
		void OnCollisionEnter2D(Collision2D collision)
		{
			Collisions.CollisionBus.Notify(gameObject, collision.gameObject, collision);
		}

		//! ----functions----
		public virtual void Prepare() { }
		public virtual void Initialize() { }
		public virtual void Clean() { }

		//! ----life cycles----
		protected virtual void Awake()
		{
			_logger = GetComponent<Logger>();
			Prepare();
		}

		protected virtual void OnDestroy()
		{
			onReleased();
		}

		//! ----object pool support----
		void OnAlloc()
		{
			Debug.Log("OnAllocated");
			var components = GetComponents<BaseComponent>();
			foreach (var component in components)
				component.Clean();
			Clean();

			Initialize();
			foreach (var component in components)
				component.Initialize();
			onInitialized();
		}

		void OnFree()
		{
			var components = GetComponents<BaseComponent>();
			foreach (var component in components)
				component.Release();
			onReleased();
		}
	}
}

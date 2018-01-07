using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace Assets.Managers
{
	public class SpawnManager : MonoBehaviour
	{
		//! ----delegates----
		public delegate void OnAllFormationSpawnedHandler();

		//! ----events----
		public static event OnAllFormationSpawnedHandler onAllFormationSpawned = delegate { };

		//! ----parameters----
		[SerializeField]
		List<string> _formationNames;
		[SerializeField]
		float _delay = 1f;

		//! ----internal----
		int _progress = 0;

		//! ----functions----
		void EmitInOrder()
		{
			if (_progress < _formationNames.Count)
			{
				Observable.Timer(TimeSpan.FromSeconds(_delay)).Subscribe(_ =>
				{
					EnemyManager.Emit(_formationNames[_progress]);
					_progress++;
				}).AddTo(gameObject);
			}
			else
				onAllFormationSpawned();
		}

		void Awake()
		{
			onAllFormationSpawned = delegate { };
		}

		void Start()
		{
			SceneConductor.onPlaySceneStart += () =>
			{
				_progress = 0;
				EnemyManager.onExtinctioned += EmitInOrder;
				EnemyManager.Emit(_formationNames[_progress]);
				_progress++;
			};

			SceneConductor.onPlaySceneEnd += () =>
			{
				EnemyManager.onExtinctioned -= EmitInOrder;
			};
			SceneConductor.onResultSceneEnd += () =>
			{
				EnemyManager.FreeAll();
			};
		}
	}
}
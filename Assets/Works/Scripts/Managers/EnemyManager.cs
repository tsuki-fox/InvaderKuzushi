using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UniRx;
using Assets.Enemies;
using TF;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Managers
{
	public class EnemyManager : MonoBehaviour
	{
		//! ----types----
		[Serializable]
		public class EmitInfo
		{
			public Vector2 position;
			public GameObject prefab;
		}
		[Serializable]
		public class Formation
		{
			public string name;
			public List<EmitInfo> infos = new List<EmitInfo>();
		}
		[Serializable]
		public class FormationSource
		{
			public string name;
			public GameObject formationRoot;
		}

		//! ----delegates----
		public delegate void OnExtinctionedHandler();

		//! ----events----
		public static event OnExtinctionedHandler onExtinctioned = delegate { };

		//! ----parameters----
		[SerializeField]
		GameObject _emitVFX;
		[SerializeField]
		AudioClip _emitSE;
		[SerializeField]
		List<FormationSource> _formationSources;
		[SerializeField]
		float _delay = 0.1f;

		//! ----internal----
		static EnemyManager _self = null;
		[SerializeField]
		List<Formation> _formations;
		static List<EnemyCore> _arriveEnemies = new List<EnemyCore>();
		static List<IDisposable> _disposers = new List<IDisposable>();

		//! ----properties----
		[Extractable]
		public static int arrives { get { return _arriveEnemies.Count; } }
		public static List<EnemyCore> arriveEnemies { get { return _arriveEnemies; } }

		//! ----functions----
		void Awake()
		{
			_arriveEnemies = new List<EnemyCore>();
			onExtinctioned = delegate { };
			_self = FindObjectOfType<EnemyManager>();
		}

		public static void Emit(string name)
		{
			var formation = _self._formations.FirstOrDefault(item => item.name == name);
			if (formation == null)
			{
				Debug.LogWarningFormat("Formation not found:name->{0}", name);
				return;
			}

			int cnt = 0;
			List<Enemies.EnemyMover> movers = new List<Enemies.EnemyMover>();
			foreach(var info in formation.infos)
			{
				var disposer = Observable.Timer(TimeSpan.FromSeconds(cnt * _self._delay)).Subscribe(_ =>
				  {
					  var enemy = ObjectPool.Alloc(info.prefab);
					  enemy.transform.position = info.position;

					  var mover = enemy.GetComponent<Enemies.EnemyMover>();
					  mover.locked = true;
					  movers.Add(mover);

					  ObjectPool.Alloc(_self._emitVFX, 1f, info.position);
					  GlobalAudioSource.PlayOneShot(_self._emitSE);

					  var core = enemy.GetComponent<EnemyCore>();
					  _arriveEnemies.Add(core);
					  core.onReleased += () =>
						{
							_arriveEnemies.Remove(core);
							if (arrives == 0)
								onExtinctioned();
						};
				  }).AddTo(_self.gameObject);
				_disposers.Add(disposer);
				cnt++;
			}

			var d = Observable.Timer(TimeSpan.FromSeconds(formation.infos.Count * _self._delay + _self._delay))
				.Subscribe(_ =>
				{
					foreach (var mover in movers)
						mover.locked = false;
				}).AddTo(_self.gameObject);
			_disposers.Add(d);
		}

		public static void FreeAll()
		{
			foreach (var disposer in _disposers)
				disposer.Dispose();
			_disposers.Clear();

			var remList = new List<EnemyCore>(_arriveEnemies);
			foreach (var item in remList)
				ObjectPool.Free(item.gameObject);

			_arriveEnemies.Clear();
		}

#if UNITY_EDITOR
		[CustomEditor(typeof(EnemyManager))]
		public class EnemyManagerInspector : Editor
		{
			string _targetName;

			void Convert()
			{
				var self = target as EnemyManager;
				self._formations = new List<Formation>();

				foreach(var source in self._formationSources)
				{
					if (source.formationRoot == null)
						continue;

					var formation = new Formation();
					formation.name = source.name;
					
					foreach(Transform elem in source.formationRoot.transform)
					{
						var info = new EmitInfo();
						info.position = elem.position;
						info.prefab = (GameObject)PrefabUtility.GetPrefabParent(elem.gameObject);
						formation.infos.Add(info);
					}
					self._formations.Add(formation);

					Debug.LogFormat("Converted formation:name->{0}", formation.name);
				}
			}

			public override void OnInspectorGUI()
			{
				var self = target as EnemyManager;

				EditorGUI.BeginChangeCheck();
				base.OnInspectorGUI();
				if (EditorGUI.EndChangeCheck())
					Convert();

				_targetName = EditorGUILayout.TextField("emit by name", _targetName);
				if (GUILayout.Button("Emit"))
					Emit(_targetName);
			}
		}
#endif
	}
}
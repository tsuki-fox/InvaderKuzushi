using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using TF;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyEmitter : MonoBehaviour
{
	//! --------type declares--------
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

	//! --------parameters--------
	[SerializeField]
	GameObject _emitVFX;
	[SerializeField]
	AudioClip _emitSE;
	[SerializeField]
	List<FormationSource> _formationSouces;
	[SerializeField]
	float _delay = 0.1f;

	//! --------internal variables--------
	[SerializeField]
	List<Formation> _formations;

	//! --------functions--------
	public void Emit(string name)
	{
		var formation = _formations.FirstOrDefault(item => item.name == name);
		if (formation == null)
		{
			Debug.LogWarningFormat("Formation not found:name->{0}", name);
			return;
		}

		int cnt = 0;
		//移動ロック用
		List<InvaderStepper> steppers = new List<InvaderStepper>();
		foreach(var info in formation.infos)
		{
			Observable.Timer(TimeSpan.FromSeconds(cnt * _delay)).Subscribe(_ =>
			{
				var enemy = ObjectPool.Alloc(info.prefab);
				enemy.transform.position = info.position;

				var stepper = enemy.GetComponent<InvaderStepper>();
				stepper.locked = true;
				steppers.Add(stepper);

				var vfx = ObjectPool.Alloc(_emitVFX);
				vfx.transform.position = info.position;
				ObjectPool.Free(vfx, 1f);

				GlobalAudioSource.PlayOneShot(_emitSE);
			}).AddTo(gameObject);
			cnt++;
		}

		//Debug.LogFormat("emitted {0} enemies", cnt);

		//全てエミットされたらロック解除
		Observable.Timer(TimeSpan.FromSeconds(formation.infos.Count * _delay + _delay)).Subscribe(_ =>
			{
//				Debug.Log("unlocked");
				foreach (var stepper in steppers)
					stepper.locked = false;
			}).AddTo(gameObject);
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(EnemyEmitter))]
	public class EnemyEmitterInspector:Editor
	{
		string _targetName;

		void ConvertToEmitInfo()
		{
			var self = target as EnemyEmitter;

			self._formations = new List<Formation>();

			foreach(var source in self._formationSouces)
			{
				if (source.formationRoot == null)
					continue;

				var formation = new Formation();
				formation.name = source.name;
				foreach(Transform elem in source.formationRoot.transform)
				{
					var info = new EmitInfo();
					info.position = elem.position;
					info.prefab =(GameObject)PrefabUtility.GetPrefabParent(elem.gameObject);
					formation.infos.Add(info);
				}
				self._formations.Add(formation);

				Debug.LogFormat("Converted fleet:name->{0}", formation.name);
			}
		}

		public override void OnInspectorGUI()
		{
			var self = target as EnemyEmitter;

			EditorGUI.BeginChangeCheck();
			base.OnInspectorGUI();
			if (EditorGUI.EndChangeCheck())
				ConvertToEmitInfo();

			_targetName = EditorGUILayout.TextField("emit by name", _targetName);

			if (GUILayout.Button("Emit"))
				self.Emit(_targetName);

		}
	}
#endif
}